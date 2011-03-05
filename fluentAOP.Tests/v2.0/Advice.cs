using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using FluentAop.Interceptors;
using System.Reflection;

namespace FluentAop.Tests.v2
{
    public class Advice
    {
        Action action;
        Action<MethodInfo> actionT;

        public Advice()
        {
            action = ()=> { return; };
        }

        public Advice(Action action)
        {
            this.action = action;
        }

        public Advice(Action<MethodInfo> actionT)
        {
            this.actionT = actionT;
        }


        public void SetAction(Action action) 
        {
            actionT = null;
            this.action = action;
        }

        public void SetAction(Action<MethodInfo> action)
        {
            action = null;
            actionT = action;
        }

        public void Run(MethodInfo mi)
        {
            if (action != null) action();
            else actionT(mi);
        }

    }
}
