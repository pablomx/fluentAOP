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
using OM = FluentAop.Tests.ObjectMother;

namespace FluentAop.Tests
{
    public class MethodWrapperTester
    {
        [Fact]
        public void Can_Utilize_Inline_Interceptors_Example_Of_How_To_Use_Nested_Closure()
        {
            var ack = 0;
            var target = new Foo();
            var proxy = new Proxy<IFoo>()
                .Target(target)
                .InterceptMethod(f => f.Return())
                .With(m =>
                {
                    m.OnBefore(() => ack++);
                    m.OnReturn(() => -1);
                })
                .Save();

            Assert.Equal(-1, proxy.Return());
            Assert.Equal(1, ack);
        }

        [Fact]
        public void Can_Utilize_Custom_Interceptors_Example_Of_How_To_Extend_MethodWrapper()
        {
            var wrapper = new CustomWrapper();
            var proxy = new Proxy<IFoo>()
                .InterceptMethod(f => f.Return())
                .With(wrapper)
                .Save();

            Assert.Equal(-1, proxy.Return());
        }

    }
}
