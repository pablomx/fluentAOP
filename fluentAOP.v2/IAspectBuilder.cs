using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;

namespace FluentAop.Poc
{
    public interface IAspectBuilder
    {
        IAspectBuilder DescribeAspect(string name);
        IAspectBuilder DescribeAspect<T>(string name) where T : Aspect, new();
        IAspectBuilder DescribeAspect<T>() where T : Aspect, new();

        IBeforeAdviceBuilderSupport Before();
        IBeforeAdviceBuilderSupport Before<T>(params Expression<Action<T>>[] methods);
        IBeforeAdviceBuilderSupport Before<T>(params Expression<Func<T, object>>[] methods);
        IAspectBuilder End();

        //IAfterThrowingAdviceBuilderSupport AfterThrowing<T>(params Expression<Action<T>>[] methods);
    }

}
