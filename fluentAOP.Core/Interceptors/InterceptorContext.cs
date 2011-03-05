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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAop.Utility;
using Proxi;
using System.Collections.ObjectModel;
using System.Collections;

namespace FluentAop.Interceptors
{
	class InterceptorContext : IInterceptor
	{
		public InterceptorStateCollection InterceptorStateCollection {get; private set;}

		public InterceptorContext(IList<InterceptorState> collection)
		{
			InterceptorStateCollection = new InterceptorStateCollection(collection);
		}

		private InterceptorState FindInterceptorStateByMethodSignature(MethodSignature signature)
		{
			return InterceptorStateCollection.Where(m => m.MethodSignature.Equals(signature)).SingleOrDefault();
		}

		#region IInterceptor Members

		object Proxi.IInterceptor.Run(IMethodInvocation mi)
		{
			// tries to find a registered method that matches...
			var interceptorState = FindInterceptorStateByMethodSignature(mi.Method.GetMethodSignature());
            
            var hasInterceptorState = interceptorState != null;
			var hasTargetObject = mi.Target != null;
            var hasTargetMethod = hasInterceptorState && interceptorState.OnInvokeWasRegistered;
            var isOpenGenericMethod = mi.Method.IsGenericMethodDefinition;

            Func<IMethodInvocation, object> call = x => mi.Method.Invoke(mi.Target, mi.Arguments);
			// method wasn't registered but can be inferred from context if a target exist
			if (!hasTargetMethod && hasTargetObject && hasInterceptorState)
            {
                interceptorState.CallbackCollection.Add(new OnInvokeCallback(call));
				hasTargetMethod = true;
            }

            #region Execution
            // executes interceptor...
			if (hasInterceptorState && hasTargetMethod) return interceptorState.Run(mi);
            // executes target: method wasn't registered but target exists, method will be inferred from context and executed on target directly...
            if (!hasTargetMethod && hasTargetObject) return call(mi);
            // unable to execute any operation...
            else throw new InvalidOperationException("Unable to execute method: " + mi.Method.Name + ". Speficy a target object or a target method utilizing Target() or OnInvoke() method.");
            #endregion
		}

		#endregion
	}
}