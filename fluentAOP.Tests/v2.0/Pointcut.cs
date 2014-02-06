using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;

namespace FluentAop.Tests.v2
{
    public class Pointcut
    {
        IList<Func<MethodInfo, bool>> rules = new List<Func<MethodInfo, bool>>();

        public Pointcut()
        {
        }

        public Pointcut(Func<MethodInfo, bool> rule)
        {
            AddRule(rule);
        }

        public void AddRule(Func<MethodInfo, bool> rule)
        {
            rules.Add(rule);
        }

        public bool Match(MethodInfo mi)
        {
            return rules.Any(rule=> rule(mi));
        }
    }
}
