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
    public interface IBeforeAdviceBuilderSupport
    {
        IAspectBuilder Do(Action action);
        IAspectBuilder Do<T1>(Action<T1> action);
        IAspectBuilder Do<T1,T2>(Action<T1,T2> action);
    }
}
