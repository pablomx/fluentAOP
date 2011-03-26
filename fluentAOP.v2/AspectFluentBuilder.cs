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
    public class AspectFluentBuilder : IAspectBuilder, IBeforeAdviceBuilderSupport //, IAfterThrowingAdviceBuilderSupport
    {
        private AspectContext context;
        private Pointcut pointcut;
        private IAdvice advice;
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

        IAspectBuilder IAspectBuilder.End()
        {
            Confirm.Assertion(aspect.Blocks.Count > 0, "Attempt to create an empty or a not well formed Aspect.");
            if(context != null) context.RegisterAspect(aspect);
            return this;
        }

        #region Do
        IAspectBuilder IBeforeAdviceBuilderSupport.Do(Action action)
        {
            advice = new Advice(action);
            SealBlock();
            return this;
        }

        IAspectBuilder IBeforeAdviceBuilderSupport.Do<T1>(Action<T1> action)
        {
            advice = new Advice<T1>(action);
            SealBlock();
            return this;
        }

        IAspectBuilder IBeforeAdviceBuilderSupport.Do<T1, T2>(Action<T1, T2> action)
        {
            advice = new Advice<T1,T2>(action);
            SealBlock();
            return this;
        }
        #endregion

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
    }
}
