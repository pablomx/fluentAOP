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
	public class OnCatchCallback : Callback
	{
        private new Action<IMethodInvocation, Exception> callback;
        private Action<Exception> callbackWithEx;

        public OnCatchCallback(Action<Exception> callback) 
        {
            this.callbackWithEx = callback;
        }

        public OnCatchCallback(Action<IMethodInvocation, Exception> callback)
        {
            this.callback = callback;
        }

        public void Run(IMethodInvocation mi, Exception ex)
        {
            if (callback != null) this.callback(mi, ex);
            else this.callbackWithEx(ex);
        }

		public override void Accept(ICallbackVisitor visitor)
		{
			visitor.Visit(this);
		}
    }
}
