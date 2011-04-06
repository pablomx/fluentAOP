using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;
using FluentAop.Utility;

namespace FluentAop.Poc
{
    public class AspectFluentBuilder : IAspectBuilder, IBeforeAdviceBuilderSupport, IDisposable //, IAfterThrowingAdviceBuilderSupport
    {
        private AspectContext context;
        private Pointcut pointcut;
        private IAdvice advice;
        private AdviceType adviceType;
        private Aspect aspect;

        #region ctors
        public AspectFluentBuilder(AspectContext context, Aspect aspect)
        {
            this.context = context;
            this.aspect = aspect;
        }
        #endregion

        #region DescribeAspect

        IAspectBuilder IAspectBuilder.DescribeAspect(string name)
        {
            return (this as IAspectBuilder).DescribeAspect<Aspect>(name);
        }

        IAspectBuilder IAspectBuilder.DescribeAspect<T>() 
        {
            return (this as IAspectBuilder).DescribeAspect<T>(string.Empty);
        }
        
        IAspectBuilder IAspectBuilder.DescribeAspect<T>(string name)
        {
            context.RegisterAspect<T>(name);
            return this;
        }

        #endregion

        #region Before
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
        #endregion

        #region Do
        IAspectBuilder IBeforeAdviceBuilderSupport.Do(Action action)
        {
            advice = new Advice(action);
            return this;
        }

        IAspectBuilder IBeforeAdviceBuilderSupport.Do<T1>(Action<T1> action)
        {
            advice = new Advice<T1>(action);
            return this;
        }

        IAspectBuilder IBeforeAdviceBuilderSupport.Do<T1, T2>(Action<T1, T2> action)
        {
            advice = new Advice<T1,T2>(action);
            return this;
        }
        #endregion

        IAspectBuilder IAspectBuilder.End()
        {
            SealBlock();
            Confirm.Assertion(aspect.Blocks.Count > 0, "Attempt to create an empty or a not well formed Aspect.");
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

        #region IDisposable Members

        public void Dispose()
        {
            context = null;
            aspect =null;;
        }

        #endregion
    }
}
