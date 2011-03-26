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
using System.Collections.Generic;

namespace FluentAop
{

	public class MethodSignature
	{
        public IList<Type> Parameters { get; private set; }
		public string MethodName { get; private set; }

		public MethodSignature(string methodName, IList<Type> parameters)
		{
            Parameters = parameters; // TODO: Must consider kind of parameter: value, reference, or output
			MethodName = methodName;
		}

		public override bool Equals(object obj)
        {
            // if object is null or type does not match return false...
            if (obj == null) return false;
            var signature = obj as MethodSignature;
            if (signature == null) return false;

            // compares params number...
			if (this.Parameters.Count != signature.Parameters.Count)
                return false;

            // compares params types...
            for (int i = 0; i < this.Parameters.Count; i++)
                if (this.Parameters[i].IsGenericParameter) continue; // open type was registered(ex: InterceptAll()) and should match any closed type
                else if (this.Parameters[i] != signature.Parameters[i])
                    return false;
            
            // compares method name...
            return this.MethodName == signature.MethodName;
        }

        public override int GetHashCode()
        {
            // XORs members...
            var hash = MethodName.GetHashCode();
            return Parameters.Aggregate(hash, (a, b) => a ^ b.GetHashCode());
        }
    }
}
