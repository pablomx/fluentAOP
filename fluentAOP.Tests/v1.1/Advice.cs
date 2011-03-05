using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using FluentAop.Interceptors;
using System.Reflection;

namespace FluentAop.Tests.v11
{
    public enum AdviceType { Before, After }

    public class Advice
    {
        Action action;
        Action<MethodInfo> actionT;

        public Advice(Action action)
        {
            this.action = action;
            action += Console.WriteLine;
        }

        public Advice(Action<MethodInfo> actionT)
        {
            this.actionT = actionT;
        }

        public void Run(MethodInfo mi)
        {
            if (action != null) action();
            else actionT(mi);
        }
    }
}
