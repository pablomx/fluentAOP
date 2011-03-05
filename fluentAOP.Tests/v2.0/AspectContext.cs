using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace FluentAop.Tests.v2
{
    public class AspectContext : IDisposable
    {
        private IDictionary<KeyValuePair<string, Type>, Aspect> registry;
        private Proxi.ProxyFactory factory = new Proxi.ProxyFactory();

        public AspectContext()
        {
            registry = new Dictionary<KeyValuePair<string, Type>, Aspect>();
        }

        public IAspectBuilder DescribeAspect() 
        {
            return DescribeAspect(string.Empty);
        }
        
        public IAspectBuilder DescribeAspect(string name)
        {
            var builder = new AspectFluentBuilder(new Aspect(name), this);
            return builder;
        }

        public void RegisterAspect<T>(T aspect) where T: Aspect
        {
            RegisterAspect(aspect.Name, aspect);
        }

        public void RegisterAspect<T>(string name, T aspect) where T : Aspect
        {
            registry.Add(new KeyValuePair<string, Type>(name, typeof(T)), aspect);
        }

        public bool Contains(string name)
        {
            return Contains<Aspect>(name);
        }

        public bool Contains<T>(string name) where T : Aspect
        {
            return registry.ContainsKey(new KeyValuePair<string, Type>(name, typeof(T)));
        }

        //public T Weave<T>(T target, params KeyValuePair<string, Type>[] keys)
        //{
        //    var aspects = registry
        //        .Where(a => keys.Any(k => k.Equals(a.Key)))
        //        .Select(a => a.Value);

        //    if (aspects.Count() <= 0) throw new InvalidOperationException();
        //    var proxy = factory.Create<T>(target, new AopInterceptor(aspects));
        //    return proxy;
        //}

        //public T Weave<T>(params string[] names) where T: new()
        //{
        //    return Weave<T>(new T(), names);
        //}


        public T Weave<T>(T target, Func<Aspect, bool> predicate) 
        {
            var aspects = registry
                .Where(a => predicate(a.Value))
                .Select(a => a.Value);
            
            var proxy = factory.Create<T>(target, new AopInterceptor(aspects));
            return proxy;
        }

        public T Weave<T>(T target, params string[] names)
        {
            if (names.Count() == 0) names = new[] { string.Empty };

            var aspects = registry
                .Where(a => names.Any(n => n == a.Key.Key))
                .Select(a => a.Value);

            if (aspects.Count() <= 0) throw new InvalidOperationException(names + " aspect is not registered. Aspect has to be registered before weaving.");
            var proxy = factory.Create<T>(target, new AopInterceptor(aspects));
            return proxy;
        }

        #region IDisposable Members

        public void Dispose()
        {
            registry = null;
        }
        #endregion
    }
}
