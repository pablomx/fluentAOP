using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Proxi;

namespace FluentAop.Poc
{
    public class AspectExecutorVisitor : IAspectVisitor
    {
        AdviceType adviceType;
        Proxi.IMethodInvocation mi;

        public AspectExecutorVisitor(AdviceType adviceType, IMethodInvocation mi)
        {
            this.adviceType = adviceType;
            this.mi = mi;
        }

        #region IAspectVisitor Members

        public void Vist(Aspect aspect)
        {
            var advices = aspect.Blocks
                 .Where(b => b.AdviceType == this.adviceType && b.Pointcut.Match(mi.Method))
                 .Select(b => b.Advice);

            foreach (var advice in advices) advice.Run(mi);
        }

        #endregion
    }
}
