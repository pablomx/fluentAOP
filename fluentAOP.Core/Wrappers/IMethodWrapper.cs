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
	public interface IMethodWrapper
	{
		CallbackCollection CallbackCollection { get; }
		void OnBefore(Action action);
		void OnBefore(Action<IMethodInvocation> action);
		void OnInvoke(Action<IMethodInvocation> action);
		void OnInvoke(Func<IMethodInvocation, object> action);
		void OnCatch(Action<Exception> action);
		void OnCatch(Action<IMethodInvocation, Exception> action);
		void OnFinally(Action action);
		void OnFinally(Action<IMethodInvocation> action);
		void OnAfter(Action action);
		void OnAfter(Action<IMethodInvocation> action);
		void OnReturn(Func<object> action);
		void OnReturn(Func<IMethodInvocation, object, object> action);
	}
}
