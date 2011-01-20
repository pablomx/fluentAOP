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
using Xunit;
using FluentAop.Utility;

namespace FluentAop.Tests
{

    public class IssuesReport
    {
        [Fact]
        public void Throws_If_Target_Is_Null()
        {
            Foo foo = null;
            var proxy = new Proxy<IFoo>()
                .Intercept(f => f.Go())
                .OnInvoke(mi => foo.Go())
                .Save();

            // TODO: Validate that the target isn't null, action.Target doesn't work, it means that the method is static.
            Assert.Throws<NullReferenceException>(() => proxy.Go()); 
        }

        [Fact]
        public void Throws_If_Target_Type_Does_Not_Have_Default_Ctor() 
        {
            // Problem only happens when proxy is built with Castle
            Assert.Throws<FluentAop.ProxyInitializationException>(()=>
            new CastleProxy<ItHasNonDefaultConstructor>()
                .InterceptMethod(c => c.Go(It.Any<string>()))
                .OnReturn(() => -1)
                .Save()
            );
        }

        [Fact]
        public void Throws_If_Method_Target_Is_Given_A_ref_or_out_Param() 
        {
            // Problem only happens when proxy is built with LinFu
            int param;
            Assert.Throws<FluentAop.ProxyInitializationException>(()=>
            new LinFuProxy<ItHasMethodWithRefParams>()
                .Intercept(c => c.ItHasAnOutParam(out param))
                .OnBefore(() => Console.Write("OnBefore"))
                .Save()
            );
        }
    }
}