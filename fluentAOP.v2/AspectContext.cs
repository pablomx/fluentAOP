using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.ObjectModel;

namespace FluentAop.Poc
{
    //TODO: thread-safe, move wave to waver, create aspectKey type
    public class AspectContext : IDisposable
    {
        private IDictionary<KeyValuePair<string, Type>, Aspect> registry;
        public IAspectWeaver Weaver {get; private set;}

        #region Ctors
        public AspectContext()
        {
            Weaver = new AspectFluentWeaver(this);
            registry = new Dictionary<KeyValuePair<string, Type>, Aspect>();
        }
        #endregion

        #region Register
        
        public void RegisterAspect<T>() where T : Aspect, new() 
        {
            RegisterAspect<T>(string.Empty);
        }
        
        public void RegisterAspect<T>(string name) where T : Aspect, new()
        {
            var aspect = new T { Name = name};
            RegisterAspect(aspect);
        }

        public void RegisterAspect<T>(T aspect) where T : Aspect
        {
            using (var describer = new AspectFluentBuilder(this, aspect))
            {
                aspect.Describe(describer);
                registry.Add(new KeyValuePair<string, Type>(aspect.Name, typeof(T)), aspect);
            }            
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

        #region IDisposable Members

        public void Dispose()
        {
            registry = null;
        }
        #endregion

        public IEnumerable<Aspect> SelectAspects(params KeyValuePair<string, Type>[] keys)
        {
            IEnumerable<Aspect> aspects = null;
            if (keys.Count() == 0) aspects = registry.Select(a => a.Value); // selects all aspects
            else aspects = registry
                .Where(a => keys.Any(k => k.Equals(a.Key)))
                .Select(a => a.Value);

            return aspects;
        }

        public IAspectBuilder DescribeAspect(string name)
        {
            var aspect = new Aspect { Name = name };
            RegisterAspect<Aspect>(aspect);            
            return new AspectFluentBuilder(this, aspect);
        }
    }
}
