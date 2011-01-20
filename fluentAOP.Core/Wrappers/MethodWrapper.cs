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

namespace FluentAop.Interceptors
{
	public class MethodWrapper : IMethodWrapper
	{
		private CallbackCollection callbackCollection = new CallbackCollection();
		public CallbackCollection CallbackCollection { get { return callbackCollection; } }

		#region IWrapper Members

		public void OnBefore(Action action)
		{
			callbackCollection.Add(new OnBeforeCallback(action));
		}

		public void OnBefore(Action<IMethodInvocation> action)
		{
			callbackCollection.Add(new OnBeforeCallback(action));
		}

		public void OnInvoke(Action<IMethodInvocation> action)
		{
			callbackCollection.Add(new OnInvokeCallback(action));
		}

		public void OnInvoke(Func<IMethodInvocation, object> action)
		{
			callbackCollection.Add(new OnInvokeCallback(action));
		}

		public void OnCatch(Action<Exception> action)
		{
			callbackCollection.Add(new OnCatchCallback(action));
		}

		public void OnCatch(Action<IMethodInvocation, Exception> action)
		{
			callbackCollection.Add(new OnCatchCallback(action));
		}

		public void OnFinally(Action action)
		{
			callbackCollection.Add(new OnFinallyCallback(action));
		}

		public void OnFinally(Action<IMethodInvocation> action)
		{
			callbackCollection.Add(new OnFinallyCallback(action));
		}

		public void OnAfter(Action action)
		{
			callbackCollection.Add(new OnAfterCallback(action));
		}

		public void OnAfter(Action<IMethodInvocation> action)
		{
			callbackCollection.Add(new OnAfterCallback(action));
		}

		public void OnReturn(Func<object> action)
		{
			callbackCollection.Add(new OnReturnCallback(action));
		}

		public void OnReturn(Func<IMethodInvocation, object, object> action)
		{
			callbackCollection.Add(new OnReturnCallback(action));
		}

		#endregion
	}
}
