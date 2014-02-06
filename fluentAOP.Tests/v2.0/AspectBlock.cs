using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace FluentAop.Tests.v2
{
    public class AspectBlock
    {
        public AdviceType AdviceType { get; private set; }
        public Pointcut Pointcut { get; private set; }
        public IAdvice Advice { get; private set; }

        public AspectBlock(AdviceType adviceType, Pointcut pointcut, IAdvice advice)
        {
            AdviceType = adviceType;
            Pointcut = pointcut;
            Advice = advice;
        }

    }
}
