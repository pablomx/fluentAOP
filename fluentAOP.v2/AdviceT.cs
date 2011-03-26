using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;
using Proxi;

namespace FluentAop.Poc
{
    public class Advice<T0> : AdviceT, IAdvice
    {
        private Action<T0> action;

        public Advice(Action<T0> action)
        {
            this.action = action;
            @params = action.Method.GetParameters();
        }

        public void Run(IMethodInvocation mi)
        {
            var arg0 = SelectArgument<T0>(0, mi);
            action.Invoke(arg0);
        }

    }

    public class Advice<T0, T1> : AdviceT, IAdvice
    {
        private Action<T0, T1> action;

        public Advice(Action<T0, T1> action)
        {
            this.action = action;
            @params = action.Method.GetParameters();
        }

        public void Run(IMethodInvocation mi)
        {
            var arg0 = SelectArgument<T0>(0, mi);
            var arg1 = SelectArgument<T1>(1, mi);
            action.Invoke(arg0, arg1);
        }

    }

    public abstract class AdviceT
    {
        protected ParameterInfo[] @params;

        protected U SelectArgument<U>(int index, IMethodInvocation mi)
        {
            if (@params[index].Name == "this" && @params[index].ParameterType.IsAssignableFrom(mi.Target.GetType())) return (U)mi.Target;
            if (@params[index].Name == "args" && @params[index].ParameterType == typeof(object[])) return (U)(mi.Arguments as object);

            var matches =
                from p in mi.Method.GetParameters()
                where p.ParameterType == @params[index].ParameterType
                && p.Name == @params[index].Name
                select p.Position;

            if (matches.Count() == 0) return default(U);
            else if (matches.Count() > 1) throw new InvalidOperationException("Ambiguous argument definition. Argument name: " + @params[index].Name);
            else return (U)mi.Arguments[matches.First()];
        }
    }
}
