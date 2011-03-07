using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;
using Proxi;

namespace FluentAop.Tests.v2
{
    public class Advice : IAdvice
    {
        private Action action;

        public Advice(Action action)
        {
            this.action = action;
        }

        public void Run(IMethodInvocation mi)
        {
            action();
        }
    }
}
