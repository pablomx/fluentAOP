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
	public static class ExpressionExtension
	{
		public static MethodCallExpression GetMethodExpression<T>(this Expression<Action<T>> method)
		{
			if (method.Body.NodeType != ExpressionType.Call) throw new ArgumentException("Method call expected", method.Body.ToString());
			return (MethodCallExpression) method.Body;
		}

		public static MethodCallExpression GetMethodExpression<T>(this Expression<Func<T, object>> exp) 
		{
            switch (exp.Body.NodeType)
            {
                case ExpressionType.Call:
                    return (MethodCallExpression) exp.Body;

                case ExpressionType.Convert:
                    var unaryExp = exp.Body as UnaryExpression;
                    Confirm.Assertion(unaryExp.Operand is MethodCallExpression, "Method expected: " + unaryExp.Operand.ToString());
                    return unaryExp.Operand as MethodCallExpression;
                    
                default: 
                    throw new InvalidOperationException("Method expected:" + exp.Body.ToString());
            }
		}

        public static MemberExpression GetMemberExpression<T>(this Expression<Func<T, object>> exp)
        {
            switch (exp.Body.NodeType)
            {
                case ExpressionType.MemberAccess: 
                    return exp.Body as MemberExpression;

                case ExpressionType.Convert:
                    var unaryExp = exp.Body as UnaryExpression;
                    Confirm.Assertion(unaryExp.Operand is MemberExpression, "Property expected: " + unaryExp.Operand.ToString());
                    return (exp.Body as UnaryExpression).Operand as MemberExpression;
                
                default: 
                    throw new ArgumentException("Property expected", exp.Body.ToString());
            }
        }

	}
}
