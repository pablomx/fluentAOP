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

namespace FluentAop.Tests.v11
{
    public class AspectFluentBuilder : IAspectBuilder, IBeforeAdviceBuilderSupport, IAfterThrowingAdviceBuilderSupport
    {
        string name;
        readonly AspectContext context;
        Pointcut pointcut;
        Advice advice;
        AdviceType adviceType;
        IList<AspectBlock> blocks;

        #region ctors
        public AspectFluentBuilder()
        {
        }

        public AspectFluentBuilder(AspectContext context)
            : this(context, string.Empty)
        {
        }

        public AspectFluentBuilder(AspectContext context, string name)
        {
            this.name = name;
            this.context = context;
        }
        #endregion

        public IBeforeAdviceBuilderSupport Before()
        {
            pointcut = new Pointcut();
            pointcut.AddRule(mi => true);
            return this;
        }

        public IBeforeAdviceBuilderSupport Before<T>(params Expression<Action<T>>[] methods)
        {
            adviceType = AdviceType.Before;
            pointcut = new Pointcut();
            foreach (var m in methods) pointcut.AddRule(mi =>
               m.GetMethodExpression()
               .Method
               .ExtractSignature()
               .Equals(m.GetMethodExpression().Method.ExtractSignature()));
            return this;
        }

        public IAfterThrowingAdviceBuilderSupport AfterThrowing<T>(params Expression<Action<T>>[] methods)
        {
            throw new NotImplementedException();
        }

        public IAspectBuilder Do(Action<Exception> ex)
        {
            throw new NotImplementedException();
        }

        public IAspectBuilder Do<T, U, V>(Action<T, U, V> action) 
        {
            throw new NotImplementedException();
        }

        public IAspectBuilder Do(Action action)
        {
            advice = new Advice(action);
            CreateBlock();
            return this;
        }

        public IAspectBuilder Do(Action<MethodInfo> action)
        {
            advice = new Advice(action);

            return this;
        }

        public void End()
        {
            context.RegisterAspect(new Aspect(blocks) { Name = name });
        }

        public void Reset()
        {
            blocks = null;
        }

        private void CreateBlock()
        {
            blocks.Add(new AspectBlock { Advice = advice, Pointcut = pointcut, AdviceType = adviceType });
            pointcut = null;
            advice = null;
        }

    }
}
