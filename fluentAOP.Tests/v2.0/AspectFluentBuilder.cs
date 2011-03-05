using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using FluentAop.Utility;
using FluentAop.Interceptors;
using System.Reflection;

namespace FluentAop.Tests.v2
{
    public class AspectFluentBuilder : IAspectBuilder, IBeforeAdviceBuilderSupport //, IAfterThrowingAdviceBuilderSupport
    {
        private AspectContext context;
        private Pointcut pointcut;
        private Advice advice;
        private AdviceType adviceType;
        private Aspect aspect;

        #region ctors
        public AspectFluentBuilder(Aspect aspect)
        {
            this.aspect = aspect;            
        }

        public AspectFluentBuilder(Aspect aspect, AspectContext context) : this(aspect)
        {
            this.context = context;
        }
        #endregion

        IBeforeAdviceBuilderSupport IAspectBuilder.Before()
        {
            pointcut = new Pointcut();
            pointcut.AddRule(mi => true);
            return this;
        }

        IBeforeAdviceBuilderSupport IAspectBuilder.Before<T>(params Expression<Action<T>>[] methods)
        {
            Require.ArgumentNotNull("methods", methods);

            adviceType = AdviceType.Before;
            pointcut = new Pointcut();
            foreach (var method in methods)
                pointcut.AddRule(mi => mi.GetMethodSignature().Equals(method.GetMethodSignature()));
            return this;
        }

        IBeforeAdviceBuilderSupport IAspectBuilder.Before<T>(params Expression<Func<T, object>>[] methods)
        {
            Require.ArgumentNotNull("methods", methods);

            adviceType = AdviceType.Before;
            pointcut = new Pointcut();
            foreach (var method in methods) 
                pointcut.AddRule(mi => mi.GetMethodSignature().Equals(method.GetMethodSignature()));
            return this;
        }

        Aspect IAspectBuilder.End()
        {
            Confirm.Assertion(aspect.Blocks.Count > 0, "Attempt to create an empty or a not well formed Aspect.");
            if(context != null) context.RegisterAspect(aspect);
            return aspect;
        }

        T IAspectBuilder.Weave<T>(T target)
        {
            (this as IAspectBuilder).End();
            var proxy = context.Weave<T>(target, aspect.Name);
            return proxy;
        }

        IAspectBuilder IBeforeAdviceBuilderSupport.Do(Action action)
        {
            advice = new Advice(action);
            SealBlock();
            return this;
        }

        IAspectBuilder IBeforeAdviceBuilderSupport.Do(Action<MethodInfo> action)
        {
            advice = new Advice(action);
            SealBlock();
            return this;
        }

        private void SealBlock()
        {
            Require.ArgumentNotNull("pointcut", pointcut);
            Require.ArgumentNotNull("advice", advice);
            aspect.Blocks.Add(new AspectBlock(adviceType, pointcut, advice));
            ResetBlock();
        }

        private void ResetBlock() 
        {
            pointcut = null;
            advice = null;        
        }

        //public IAfterThrowingAdviceBuilderSupport AfterThrowing<T>(params Expression<Action<T>>[] methods)
        //{
        //    throw new NotImplementedException();
        //}

        //public IAspectBuilder Do(Action<Exception> ex)
        //{
        //    throw new NotImplementedException();
        //}

        //public IAspectBuilder Do<T, U, V>(Action<T, U, V> action) 
        //{
        //    throw new NotImplementedException();
        //}
    }
}
