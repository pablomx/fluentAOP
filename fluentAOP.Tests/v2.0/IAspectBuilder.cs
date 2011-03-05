using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

using FluentAop.Utility;
using FluentAop.Interceptors;
using System.Reflection;

namespace FluentAop.Tests.v2
{
    public interface IAspectBuilder
    {
        IBeforeAdviceBuilderSupport Before();
        IBeforeAdviceBuilderSupport Before<T>(params Expression<Action<T>>[] methods);
        IBeforeAdviceBuilderSupport Before<T>(params Expression<Func<T, object>>[] methods);
        T Weave<T>(T target);
        Aspect End();

        //IAfterThrowingAdviceBuilderSupport AfterThrowing<T>(params Expression<Action<T>>[] methods);

        //IAspectBuilder Before<T>(params Expression<Func<T, object>>[] methods);
        //IAspectBuilder Before<T>(params Expression[] methods);

        //IAspectBuilder Do(Action action);
        //IAspectBuilder Do(Action<object[]> action);
        //IAspectBuilder Do(Action<MethodInfo> action);
    }

}
