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
    public interface IBeforeAdviceBuilderSupport
    {
        IAspectBuilder Do(Action action);
        IAspectBuilder Do(Action<MethodInfo> action);
    }
}
