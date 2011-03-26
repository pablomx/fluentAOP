using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;

namespace FluentAop.Poc
{
    public interface IAfterThrowingAdviceBuilderSupport
    {
        IAspectBuilder Do(Action<Exception> ex);
        IAspectBuilder Do<T, U, V>(Action<T, U, V> action);
    }
}
