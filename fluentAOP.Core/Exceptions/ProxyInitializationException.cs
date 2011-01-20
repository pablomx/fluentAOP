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
using System.Linq.Expressions;
using System.Collections.Generic;
using Proxi;
using FluentAop.Utility;
using FluentAop.Interceptors;
using System.Runtime.Serialization;

namespace FluentAop
{
	[Serializable]
	public class ProxyInitializationException : Exception
	{
		public ProxyInitializationException() { }
		public ProxyInitializationException(string message) : base(message) { }
		public ProxyInitializationException(string message, Exception innerException) : base(message, innerException) { }
		protected ProxyInitializationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
