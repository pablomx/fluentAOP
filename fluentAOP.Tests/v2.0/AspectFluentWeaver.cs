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
    public class AspectFluentWeaver : IAspectWeaver
    {

        private List<KeyValuePair<string, Type>> keys;
        private AspectContext context;


        public AspectFluentWeaver(AspectContext context)
        {
            this.context = context;
            keys = new List<KeyValuePair<string, Type>>();
        }

        public AspectFluentWeaver Attach(string name)
        {
            keys.Add(new KeyValuePair<string, Type>(name, typeof(Aspect)));
            return this;
        }

        public AspectFluentWeaver Attach<T>() where T : Aspect
        {
            return Attach<T>(string.Empty);
        }

        public AspectFluentWeaver Attach<T>(string name) where T : Aspect
        {
            keys.Add(new KeyValuePair<string, Type>(name, typeof(T)));
            return this;
        }

        public T To<T>(T t) where T : class
        {
            var instance = context.Weave<T>(t, keys.ToArray());

            keys.Clear();
            return instance;
        }

        public T To<T>() where T : class, new()
        {
            return To<T>(new T());
        }

    }
}
