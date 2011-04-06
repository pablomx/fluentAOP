using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAop.Poc
{
    public static class It
    {
        public static T Param<T>()
        {
            return default(T);
        }

        public static KeyValuePair<string, Type> Aspect(string name)
        {
            return new KeyValuePair<string, Type>(name, typeof(Aspect));
        }

        public static KeyValuePair<string, Type> Aspect<T>() where T : Aspect
        {
            return new KeyValuePair<string, Type>(string.Empty, typeof(T));
        }

        public static KeyValuePair<string, Type> Aspect<T>(string name) where T : Aspect 
        {
            return new KeyValuePair<string, Type>(name, typeof(T));
        }
    }

}
