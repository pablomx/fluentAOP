using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Proxi;

namespace FluentAop.Poc
{
    public class AopInterceptor : Proxi.InterceptorBase
    {
        private IEnumerable<Aspect> aspects;

        public AopInterceptor(IEnumerable<Aspect> aspects)
        {
            this.aspects = aspects;
        }

        protected override void OnBefore(IMethodInvocation mi)
        {
            var visitor = new AspectExecutorVisitor(AdviceType.Before, mi);
            foreach (var aspect in aspects) aspect.AcceptVisitor(visitor);
        }

    }
}
