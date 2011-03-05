using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAop.Tests.v2
{
    public class AopInterceptor : Proxi.InterceptorBase
    {
        private IEnumerable<Aspect> aspects;

        public AopInterceptor(IEnumerable<Aspect> aspects)
        {
            this.aspects = aspects;
        }

        protected override void OnBefore(Proxi.IMethodInvocation mi)
        {
            var visitor = new AspectExecutorVisitor(AdviceType.Before, mi.Method);
            foreach (var aspect in aspects) aspect.AcceptVisitor(visitor);
        }

    }
}
