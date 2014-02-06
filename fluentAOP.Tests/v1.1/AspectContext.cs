using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace FluentAop.Tests.v11
{
    public class AspectContext
    {
        IDictionary<KeyValuePair<string, Type>, Aspect> registry = new Dictionary<KeyValuePair<string, Type>, Aspect>();

        public AspectContext()
        {
        }

        public IAspectBuilder CreateAspect(string name)
        {
            return new AspectFluentBuilder(this, name);
        }

        public IAspectBuilder CreateAspect()
        {
            return new AspectFluentBuilder(this);
        }

        public void RegisterAspect<T>(T aspect) where T: Aspect
        {
            registry.Add(new KeyValuePair<string, Type>(aspect.Name, typeof(T)), aspect);
        }

    }
}
