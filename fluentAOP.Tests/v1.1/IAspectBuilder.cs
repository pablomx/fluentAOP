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

namespace FluentAop.Tests.v11
{
    public interface IAfterThrowingAdviceBuilderSupport 
    {
        IAspectBuilder Do(Action<Exception> ex);

        IAspectBuilder Do<T, U, V>(Action<T, U, V> action);
    }

    public interface IBeforeAdviceBuilderSupport 
    
    {
        IAspectBuilder Do(Action action);
        //IAspectBuilder Do(Action<object[]> action);
        IAspectBuilder Do(Action<MethodInfo> action);
        
    }

    public interface IAspectBuilder
    {
        IBeforeAdviceBuilderSupport Before();
        IBeforeAdviceBuilderSupport Before<T>(params Expression<Action<T>>[] methods);
        
        IAfterThrowingAdviceBuilderSupport AfterThrowing<T>(params Expression<Action<T>>[] methods);

        //IAspectBuilder Before<T>(params Expression<Func<T, object>>[] methods);
        //IAspectBuilder Before<T>(params Expression[] methods);

        //IAspectBuilder Do(Action action);
        //IAspectBuilder Do(Action<object[]> action);
        //IAspectBuilder Do(Action<MethodInfo> action);
    }

    public static class AspectBuilderExt
    {
        public static void End(this IAspectBuilder @this)
        {
        }
    }
}
