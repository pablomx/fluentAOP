using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace FluentAop.Tests.v11
{
    public class AspectBlock
    {
        //after(FigureElement fe, int x, int y) returning:
        //  ...SomePointcut... {
        //      ...SomeBody...
        //}

        public AdviceType AdviceType { get; set; }
        public Pointcut Pointcut { get; set; }
        public Advice Advice { get; set; }
    }
}
