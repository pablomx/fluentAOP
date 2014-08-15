/* •——————————————————————————————————————————————————————————————————————————•
   | Copyright © Pablo Orozco (pablo@orozco.me).                              |
   | All rights reserved.                                                     |
   |                                                                          |
   | Licensed under the Apache License, Version 2.0 (the "License");          |
   | you may not use this file except in compliance with the License.         |
   | You may obtain a copy of the License at                                  |
   |                                                                          |
   | http://www.apache.org/licenses/LICENSE-2.0                               |
   |                                                                          |
   | Unless required by applicable law or agreed to in writing, software      |
   | distributed under the License is distributed on an "AS IS" BASIS,        |
   | WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. |
   | See the License for the specific language governing permissions and      |
   | limitations under the License.                                           |
   •——————————————————————————————————————————————————————————————————————————• */

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Proxi;
using FluentAop.Utility;
using FluentAop.Interceptors;
using System.Runtime.Serialization;
using CilBuilder = Proxi.ProxyFactory;

namespace FluentAop
{
    public class Proxy<T>
	{

        #region Properties and Fields
		private InterceptorContext context;
		private InterceptorStateCollection states;
        private T proxy;
		private List<Type> implementedTypes = new List<Type>();
        private IDictionary<MethodSignature, CallbackCollection> register = new Dictionary<MethodSignature, CallbackCollection>();
        private List<MethodSignature> interceptedMembers = new List<MethodSignature>();
		private readonly bool isInterface;
        private CilBuilder cilBuilder;
        protected T target;
		protected virtual ProxyBehavior Behavior { get { return ProxyBehavior.Default; } }
		#endregion

        #region Constructors
		public Proxy()
        {
            isInterface = typeof(T).IsInterface;
        }

        #endregion

		#region Public Members

        public virtual CilBuilder CilBuilder 
        {
            get 
            {
                if (cilBuilder == null) cilBuilder = new CilBuilder(Behavior);
                return cilBuilder;
            }

            set 
            { 
                cilBuilder = value; 
            } 
        }

		public Proxy<T> Intercept(Expression<Action<T>> method)
		{
			return InterceptMethod(method);
		}

		public Proxy<T> Intercept(Expression<Func<T, object>> method)
		{
			return InterceptMethod(method);
		}

		public Proxy<T> InterceptAllGetters()
		{
			var methods = typeof(T)
				.GetMethodsToIntercept()
				.SelectGetters();
			RegisterSignatures(methods.Select(m => m.ExtractSignature()));
			return this;
		}

		public Proxy<T> InterceptAllSetters()
		{
			var methods = typeof(T)
				.GetMethodsToIntercept()
				.SelectSetters();
			RegisterSignatures(methods.Select(m => m.ExtractSignature()));
			return this;
		}

		public Proxy<T> InterceptAll()
		{
			var methods = typeof(T).GetMethodsToIntercept();
			RegisterSignatures(methods.Select(m => m.ExtractSignature()));
			return this;
		}

        public Proxy<T> InterceptMethods(params Expression<Action<T>>[] methods)
        {
            Require.ArgumentNotNull("methods", methods);
            var signatures = methods.Select(p => p.GetMethodExpression().Method.ExtractSignature());
            RegisterSignatures(signatures);
            return this;
        }

        public Proxy<T> InterceptWhere(Func<System.Reflection.MethodInfo, bool> predicate)
        {
            Require.ArgumentNotNull("predicate", predicate);
            var methods = typeof(T)
                .GetMethodsToIntercept()
                .Where(m => predicate(m));
            RegisterSignatures(methods.Select(m => m.ExtractSignature()));
            return this;
        }

        public Proxy<T> InterceptMethod(Expression<Action<T>> method)
		{
			Require.ArgumentNotNull("method", method);
			RegisterMethods(method.GetMethodExpression());
			return this;
		}

		public Proxy<T> InterceptMethod(Expression<Func<T, object>> method)
		{
			Require.ArgumentNotNull("method", method);
			RegisterMethods(method.GetMethodExpression());
			return this;
		}

		public Proxy<T> InterceptGetter(Expression<Func<T, object>> getter)
		{
			RegisterProperty(getter, Type.EmptyTypes, "get_");
			return this;
		}

		public Proxy<T> InterceptSetter(Expression<Func<T, object>> setter)
		{
			RegisterProperty(setter, new[] { setter.Body.Type }, "set_");
			return this;
		}

		public Proxy<T> Implement(params Type[] typesToImplement)
		{
			Require.ArgumentNotNull("typesToImplement", typesToImplement);
			typesToImplement.ForEach(t => implementedTypes.Add(t));
			return this;
		}

		public Proxy<T> Implement<TInterface>()	where TInterface : class
		{
			implementedTypes.Add(typeof(TInterface));
			return this;
		}

		public Proxy<T> Target(T targetObject)
		{
			Require.ArgumentNotNull("target", targetObject);
			this.target = targetObject;
			return this;
		}

		public Proxy<T> OnBefore(Action action)
		{
			Require.ArgumentNotNull("action", action);
            var callback = new OnBeforeCallback(action);            
            interceptedMembers.ForEach(ms => register[ms].Add(callback));
			return this;
		}

		public Proxy<T> OnBefore(Action<IMethodInvocation> action)
		{
			Require.ArgumentNotNull("action", action);
            var callback = new OnBeforeCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
			return this;
		}

		public Proxy<T> OnInvoke(Func<IMethodInvocation, object> action)
		{
			Require.ArgumentNotNull("action", action);
			var callback = new OnInvokeCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
			return this;
		}

		public Proxy<T> OnInvoke(Action<IMethodInvocation> action)
		{
			Require.ArgumentNotNull("action", action);
			var callback = new OnInvokeCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
			return this;
		}

        public Proxy<T> OnCatch(Action<Exception> action)
        {
            Require.ArgumentNotNull("action", action);
            var callback = new OnCatchCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
            return this;
        }

        public Proxy<T> OnCatch(Action<IMethodInvocation, Exception> action)
        {
            Require.ArgumentNotNull("action", action);
            var callback = new OnCatchCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
            return this;
        }

        public Proxy<T> OnFinally(Action action)
        {
            Require.ArgumentNotNull("action", action);
            var callback = new OnFinallyCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
            return this;
        }

        public Proxy<T> OnFinally(Action<IMethodInvocation> action)
        {
            Require.ArgumentNotNull("action", action);
            var callback = new OnFinallyCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
            return this;
        }

        public Proxy<T> OnAfter(Action action)
        {
            Require.ArgumentNotNull("action", action);
            var callback = new OnAfterCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
            return this;
        }

		public Proxy<T> OnAfter(Action<IMethodInvocation> action)
		{
			Require.ArgumentNotNull("action", action);
            var callback = new OnAfterCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
            return this;
		}

		public Proxy<T> OnReturn(Func<object> action)
		{
			Require.ArgumentNotNull("action", action);
            var callback = new OnReturnCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
            return this;
		}

		public Proxy<T> OnReturn(Func<IMethodInvocation,object,object> action)
		{
			Require.ArgumentNotNull("action", action);
            var callback = new OnReturnCallback(action);
            interceptedMembers.ForEach(e => register[e].Add(callback));
            return this;
		}

		public Proxy<T> With(Action<IMethodWrapper> action)
		{
			var wrapper = new MethodWrapper();
			action(wrapper);
			wrapper.CallbackCollection.ForEach(callback => interceptedMembers.ForEach(e => register[e].Add(callback)));
			return this;
		}

		public Proxy<T> With<TMethodWrapper>(TMethodWrapper wrapper) where TMethodWrapper : IMethodWrapper
		{
			wrapper.CallbackCollection.ForEach(callback => interceptedMembers.ForEach(e => register[e].Add(callback)));
			return this;
		}

		public T Save()
		{
			try
			{
				if (proxy == null)
				{
					Confirm.Assertion(register.Keys.Count > 0, "Proxy definition must specify what members have to be intercepted. Specify one or more members utilizing Intercept*() methods.");
					Confirm.Assertion(register.All(vp => vp.Value.Count > 0), "One or more intercepted members do not specify how to handle interception. Specify interception behavior utilizing On*() methods.");
                    states = new InterceptorStateCollection();
                    foreach (var key in register.Keys) states.Add(new InterceptorState(key, register[key]));
                    context = new InterceptorContext(states);
                    proxy = (target == null) ?
                        CilBuilder.Create<T>(context, implementedTypes.ToArray()) :
                        CilBuilder.Create<T>(target, context, implementedTypes.ToArray()); 
				}
			}
			catch (Exception ex)
			{
				throw new ProxyInitializationException("Unable to create proxy: " + ex.Message, ex);
			}

			return proxy;
		}

		#endregion

		#region Private Methods

		private void RegisterProperty(Expression<Func<T, object>> expression, Type[] parameters, string prefix)
		{
			Require.ArgumentNotNull("property", expression);
			var exp = expression.GetMemberExpression();
			var methodName = prefix + exp.Member.Name;
			var signature = new MethodSignature (methodName, parameters);
			if (!isInterface) Require.OverridableProperty(typeof(T), signature.MethodName);
			RegisterSignatures(new[] { signature }); 
		}

		private void RegisterMethods(params MethodCallExpression[] methods)
		{
            if (!isInterface) methods.ForEach(m => Require.OverridableMethod(m));
            RegisterSignatures(methods.Select(m => m.Method.ExtractSignature()));
		}

		private void RegisterSignatures(IEnumerable<MethodSignature> signatures)
		{
            interceptedMembers = new List<MethodSignature>(signatures);
            foreach (var signature in interceptedMembers) 
            {
                if(!register.ContainsKey(signature))
                    register.Add(signature, new CallbackCollection());
            }
		}
		#endregion
    }
}