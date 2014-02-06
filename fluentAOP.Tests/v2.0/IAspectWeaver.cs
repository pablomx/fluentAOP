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
    public interface IAspectWeaver
    {
        AspectFluentWeaver Attach(string name);
        AspectFluentWeaver Attach<T>() where T : Aspect;
        AspectFluentWeaver Attach<T>(string name) where T : Aspect;
        T To<T>(T t) where T : class;
        T To<T>() where T : class, new();
    }
}
