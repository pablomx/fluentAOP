using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace FluentAop.Tests.v11
{

    public class Aspect
    {
        public string Name { get; set; }

        IList<AspectBlock> blocks = new List<AspectBlock>();

        public Aspect(IList<AspectBlock> blocks)
        {
            this.blocks = blocks;
        }

        public Aspect(AdviceType adviceType, Pointcut pointcut, Advice advice)
        {
            blocks.Add(new AspectBlock { AdviceType = adviceType, Pointcut = pointcut, Advice = advice });
        }

        public void Append(AdviceType adviceType, Pointcut pointcut, Advice advice) 
        {
            blocks.Add(new AspectBlock {AdviceType=adviceType, Pointcut = pointcut, Advice = advice });
        }
    }
}
