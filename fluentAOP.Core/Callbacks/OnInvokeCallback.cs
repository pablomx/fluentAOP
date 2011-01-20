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
using System.Collections;

namespace FluentAop.Interceptors
{
	public class OnInvokeCallback : Callback
    {
		private new Action<IMethodInvocation> callback;
		private Func<IMethodInvocation, object> callbackWithReturn;

		public OnInvokeCallback(Action<IMethodInvocation> callback)
		{
			this.callback = callback;
		}

        public OnInvokeCallback(Func<IMethodInvocation, object> callback)
        {
			this.callbackWithReturn = callback;
        }

        public object Run(IMethodInvocation mi)
        {
			if (callbackWithReturn != null) return this.callbackWithReturn(mi);
			callback(mi);
			return null;
        }

		public override void Accept(ICallbackVisitor visitor)
		{
			visitor.Visit(this);
		}
    }
}
