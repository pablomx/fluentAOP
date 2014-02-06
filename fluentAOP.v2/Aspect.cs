using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace FluentAop.Poc
{

    public class Aspect
    {
        public string Name { get; set; }
        public IList<AspectBlock> Blocks { get; private set; }

        public Aspect()
        {
            Name = string.Empty;
            Blocks = new List<AspectBlock>();
        }

        public virtual void Describe(IAspectBuilder describer) 
        {        
        }

        public Aspect Append(AdviceType adviceType, Pointcut pointcut, Advice advice) 
        {
            Blocks.Add(new AspectBlock(adviceType, pointcut, advice));
            return this;
        }

        public void AcceptVisitor(IAspectVisitor visitor)
        {
            visitor.Vist(this);
        }
    }
}
