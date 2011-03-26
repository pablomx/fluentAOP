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
	public static class Confirm
	{
		#region Assertions

		public static void Assertion(Func<bool> predicate)
		{
			Assertion(predicate());
		}

		public static void Assertion(Func<bool> predicate, string message)
		{
			Assertion(predicate(), new InvalidOperationException(message));
		}

		public static void Assertion(Func<bool> predicate, Exception ex)
		{
			if (!predicate()) throw ex;
		}

		public static void Assertion(bool condition)
		{
			Assertion(condition, "Unable to assert statement. This usually happens when an invalid state is detected in the application.");
		}

		public static void Assertion(bool condition, string message)
		{
			Assertion(condition, new InvalidOperationException(message));
		}

		public static void Assertion(bool condition, Exception ex)
		{
			if (!condition) throw ex;
		}

		#endregion
	}
}
