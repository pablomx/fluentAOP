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
	class SelectCallbackVisitor : ICallbackVisitor
	{
        private IList<OnBeforeCallback> onBeforeCallbacks = new List<OnBeforeCallback>();
		private IList<OnInvokeCallback> onInvokeCallbacks = new List<OnInvokeCallback>();
		private IList<OnCatchCallback> onCatchCallbacks = new List<OnCatchCallback>();
		private IList<OnFinallyCallback> onFinallyCallbacks = new List<OnFinallyCallback>();
		private IList<OnAfterCallback> onAfterCallbacks = new List<OnAfterCallback>();
		private IList<OnReturnCallback> onReturnCallbacks = new List<OnReturnCallback>();

		public IEnumerable<OnBeforeCallback> OnBeforeCallbackCollection { get { return onBeforeCallbacks; } }
		public IEnumerable<OnInvokeCallback> OnInvokeCallbackCollection { get { return onInvokeCallbacks; } }
		public IEnumerable<OnCatchCallback> OnCatchCallbackCollection { get { return onCatchCallbacks; } }
		public IEnumerable<OnFinallyCallback> OnFinallyCallbackCollection { get { return onFinallyCallbacks; } }
		public IEnumerable<OnAfterCallback> OnAfterCallbackCollection { get { return onAfterCallbacks; } }
		public IEnumerable<OnReturnCallback> OnReturnCallbackCollection { get { return onReturnCallbacks; } }

		#region ICallbackVisitor Members

		public void Visit(OnBeforeCallback callback)
		{
			onBeforeCallbacks.Add(callback);
		}

		public void Visit(OnInvokeCallback callback)
		{
			onInvokeCallbacks.Add(callback);
		}

		public void Visit(OnCatchCallback callback)
		{
			onCatchCallbacks.Add(callback);
		}

		public void Visit(OnFinallyCallback callback)
		{
			onFinallyCallbacks.Add(callback);
		}

		public void Visit(OnAfterCallback callback)
		{
			onAfterCallbacks.Add(callback);
		}

		public void Visit(OnReturnCallback callback)
		{
			onReturnCallbacks.Add(callback);
		}

		#endregion
	}
}
