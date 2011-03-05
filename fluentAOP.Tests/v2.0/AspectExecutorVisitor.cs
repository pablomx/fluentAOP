using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAop.Tests.v2
{
    public class AspectExecutorVisitor : IAspectVisitor
    {
        AdviceType adviceType;
        MethodInfo mi;

        public AspectExecutorVisitor(AdviceType adviceType, MethodInfo mi)
        {
            this.adviceType = adviceType;
            this.mi = mi;
        }

        #region IAspectVisitor Members

        public void Vist(Aspect aspect)
        {
            var advices = aspect.Blocks
                 .Where(b => b.AdviceType == this.adviceType && b.Pointcut.Match(mi))
                 .Select(b => b.Advice);

            foreach (var advice in advices) advice.Run(mi);
        }

        #endregion
    }
}
