/* •——————————————————————————————————————————————————————————————————————————•
   | Copyright © Pablo Orozco (pablo@orozco.me).                              |
   | All rights reserved.                                                     |
   |                                                                          |
   | Licensed under the Apache License, Version 2.0 (the "License");          |
   | you may not use this file except in compliance with the License.         |
   | You may obtain a copy of the License at                                  |
   |                                                                          |
   | http://www.apache.org/licenses/LICENSE-2.0                               |
   |                                                                          |
   | Unless required by applicable law or agreed to in writing, software      |
   | distributed under the License is distributed on an "AS IS" BASIS,        |
   | WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. |
   | See the License for the specific language governing permissions and      |
   | limitations under the License.                                           |
   •——————————————————————————————————————————————————————————————————————————• */

using System;
using System.Linq;
using System.Reflection;

namespace FluentAop.Utility
{
	public static class MethodInfoExtension
	{

        public static MethodInfo[] SelectOverridableMethods(this MethodInfo[] methods)
        {
            return methods.Where(m => m.IsVirtual && !m.IsFinal).ToArray();
        }

        public static MethodInfo[] SelectGetters(this MethodInfo[] methods)
        {
			return methods.Where(m => m.Name.StartsWith("get_", StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public static MethodInfo[] SelectSetters(this MethodInfo[] methods) 
        {
            return methods.Where(m => m.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase)).ToArray();
        }

		public static Type[] GetMethodParameterTypes(this MethodInfo method)
		{
			return (from t in method.GetParameters()
					select t.ParameterType).ToArray();
		}

        public static MethodSignature ExtractSignature(this MethodInfo info)
        {
            return new MethodSignature(info.Name, info.GetMethodParameterTypes());
        }

        public static MethodInfo[] GetMethodsToIntercept(this Type type)
        {
            return type.IsInterface ?
                type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                : type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
				.SelectOverridableMethods();
		}

	}
}
