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

    class InterceptorState : InterceptorBase, IInterceptorState
	{
        public CallbackCollection CallbackCollection { get; private set; }
		public MethodSignature MethodSignature { get; private set; }
		private SelectCallbackVisitor visitor;

        public bool OnInvokeWasRegistered 
        { 
            get 
            {
                return CallbackCollection
                .Where(c => c is OnInvokeCallback)
                .Count() > 0;
            } 
        }

        public InterceptorState(MethodSignature ms, CallbackCollection callbacks)
		{
			this.MethodSignature = ms;
            this.CallbackCollection = callbacks;
			this.visitor = new SelectCallbackVisitor();

		}

		#region Overrided Members

		protected override void OnBefore(IMethodInvocation mi)
		{
			CallbackCollection.ForEach(c => c.Accept(visitor));
			visitor.OnBeforeCallbackCollection.ForEach(c => c.Run(mi));
		}

		protected override object OnInvoke(IMethodInvocation mi)
		{
			object returnValue = null;
			visitor.OnInvokeCallbackCollection.ForEach(c => returnValue = c.Run(mi));
			return returnValue;
		}

		protected override void OnCatch(IMethodInvocation mi, Exception ex)
		{ 
			var collection = visitor.OnCatchCallbackCollection as IList<OnCatchCallback>;
			if (collection.Count == 0) throw ex; // if none callback was defined ex must be thrown
			collection.ForEach(c => c.Run(mi, ex));
		}

		protected override void OnFinally(IMethodInvocation mi)
		{
			visitor.OnFinallyCallbackCollection.ForEach(c => c.Run(mi));
		}

		protected override void OnAfter(IMethodInvocation mi)
		{
			visitor.OnAfterCallbackCollection.ForEach(c => c.Run(mi));
		}

		protected override object OnReturn(IMethodInvocation mi, object result)
		{
			var collection = visitor.OnReturnCallbackCollection as IList<OnReturnCallback>;
			if (collection.Count == 0) return result; // if none callback was defined return result

			object returnValue = null;
			visitor.OnReturnCallbackCollection.ForEach(c => returnValue = c.Run(mi, result));
			return returnValue;
		}
		#endregion

	}
}
