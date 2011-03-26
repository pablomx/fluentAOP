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
using System.Linq.Expressions;

namespace FluentAop.Utility
{
    public static class Require
    {
        #region Arguments
        public static void ArgumentNotNull(string name, object value)
        {
            if (value == null) throw new ArgumentNullException(name);
        }
        #endregion

        #region Members
        public static void OverridableMethod(MethodCallExpression exp)
        {
            if (!exp.Method.IsVirtual || exp.Method.IsFinal)
                throw new InvalidOperationException(string.Concat("Method must be virtual and not final: ", exp.Method.Name, "."));
        }

        public static void OverridableProperty(Type type, string property)
        {
			if (!type.GetMethod(property).IsVirtual || type.GetMethod(property).IsFinal)
				throw new InvalidOperationException(string.Concat("Property must be virtual and not final: ", type.FullName, ".", property, "."));
        }

        #endregion        
		
	}
}