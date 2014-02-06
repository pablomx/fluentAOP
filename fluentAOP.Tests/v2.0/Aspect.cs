using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace FluentAop.Tests.v2
{

    public class FooAspect : Aspect 
    {
        public FooAspect()
        {
            Name = "Logging";
            Append(AdviceType.Before, new Pointcut(), new Advice(() => { return; }));
            Append(AdviceType.Before, new Pointcut(), new Advice(() => { return; }));

            DescribeAspect("logging")
            .Before()
            .Do(()=>
            {
                Console.Write("");
            }).End();
        }

    }

    public class Aspect
    {
        public string Name { get; protected set; }
        public IList<AspectBlock> Blocks { get; private set; }

        #region Ctors

        public Aspect() : this(string.Empty)
        {
        }


        public Aspect(IList<AspectBlock> blocks) : this(string.Empty, blocks)
        {
        }

        public Aspect(string name) : this(name, new List<AspectBlock>())
        {
        }

        public Aspect(string name, IList<AspectBlock> blocks)
        {
            Name = name;
            Blocks = blocks;
        }

        #endregion

        protected IAspectBuilder DescribeAspect(string name)
        {
            Name = name;
            return new AspectFluentBuilder(this);
        }

        public Aspect Append(AdviceType adviceType, Pointcut pointcut, Advice advice) 
        {
            Blocks.Add(new AspectBlock(adviceType, pointcut, advice));
            return this;
        }

        public Aspect Append(IEnumerable<AspectBlock> blocks) 
        {
            Blocks.Concat<AspectBlock>(blocks);
            return this;
        }

        public void AcceptVisitor(IAspectVisitor visitor)
        {
            visitor.Vist(this);
        }
    }
}
