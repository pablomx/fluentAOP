using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace FluentAop.Tests.v2
{
    //TODO: thread-safe, move wave to waver, create aspectKey type
    public class AspectContext : IDisposable
    {
        private IDictionary<KeyValuePair<string, Type>, Aspect> registry;
        private Proxi.ProxyFactory factory = new Proxi.ProxyFactory();
        public IAspectWeaver Weaver {get; private set;}

        #region Ctors
        public AspectContext()
        {
            Weaver = new AspectFluentWeaver(this);
            registry = new Dictionary<KeyValuePair<string, Type>, Aspect>();
        }
        #endregion

        #region Describe
        public IAspectBuilder DescribeAspect()
        {
            return DescribeAspect(string.Empty);
        }

        public IAspectBuilder DescribeAspect(string name)
        {
            var builder = new AspectFluentBuilder(new Aspect(name), this);
            return builder;
        }
        #endregion

        #region Register
        public void RegisterAspect<T>(T aspect) where T : Aspect
        {
            RegisterAspect(aspect.Name, aspect);
        }

        public void RegisterAspect<T>(string name, T aspect) where T : Aspect
        {
            registry.Add(new KeyValuePair<string, Type>(name, typeof(T)), aspect);
        }
        #endregion

        #region Contains
        public bool Contains(string name)
        {
            return Contains<Aspect>(name);
        }

        public bool Contains<T>(string name) where T : Aspect
        {
            return registry.ContainsKey(new KeyValuePair<string, Type>(name, typeof(T)));
        }
        #endregion

        #region Weave

        public T Weave<T>(params KeyValuePair<string, Type>[] keys) where T : new() 
        {
            return Weave<T>(new T(), keys);
        }

        public T Weave<T>(T target, params KeyValuePair<string, Type>[] keys)
        {
            IEnumerable<Aspect> aspects = null;
            if (keys.Count() == 0) aspects = registry.Select(a => a.Value); // selects all aspects
            else aspects = registry
                .Where(a => keys.Any(k => k.Equals(a.Key)))
                .Select(a => a.Value);

            if (aspects.Count() == 0) throw new InvalidOperationException("");
            var proxy = factory.Create<T>(target, new AopInterceptor(aspects));
            return proxy;
        }

        public T Weave<T>(T target, Func<Aspect, bool> predicate)
        {
            var aspects = registry
                .Where(a => predicate(a.Value))
                .Select(a => a.Value);

            var proxy = factory.Create<T>(target, new AopInterceptor(aspects));
            return proxy;
        }

        public T Weave<T>(Func<Aspect, bool> predicate) where T: new()
        {
            return Weave<T>(new T(), predicate);
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            registry = null;
        }
        #endregion
    }
}
