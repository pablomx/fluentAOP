using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

using FluentAop.Utility;
using System.Reflection;

namespace FluentAop.Tests.v11
{
    public interface IPointcut 
    {
        bool Match(MethodInfo mi);
    }

    public class Pointcut : IPointcut
    {
        IList<Func<MethodInfo, bool>> rules = new List<Func<MethodInfo, bool>>();

        public void AddRule(Func<MethodInfo, bool> rule)
        {
            rules.Add(rule);
        }

        public bool Match(MethodInfo mi)
        {
            return rules.Any(rule=> rule(mi));
        }

        //IList<MethodSignature> signatures = new List<MethodSignature>();
        //public void Register<T>(Expression<Action<T>> action) 
        //{
        //    signatures.Add(
        //        action
        //        .GetMethodExpression()
        //        .Method
        //        .ExtractSignature());
        //}

    }
}
