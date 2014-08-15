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
    public class VirtualMethodInterceptionTester
    {
        [Fact]
        public void Can_Intercept_Virtual_Methods()
        {
            var foo = new Foo();
            var proxy = new Proxy<Foo>()
                .Target(foo)
                .InterceptMethod(f => f.Go())
                .OnBefore(() => Assert.False(foo.WasExecuted))
                .Save();

            proxy.Go();
            Assert.True(foo.WasExecuted);
        }

        [Fact]
        public void Can_Intercept_Virtual_Properties()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<Foo>()
                .Target(foo)
                .InterceptSetter(f => f.Name)
                .OnBefore(() => Assert.False(foo.WasExecuted))
                .Save();

            proxy.Name = string.Empty;
            Assert.True(foo.WasExecuted);
        }

        [Fact]
        public void Should_Throw_If_Intercepted_Method_Is_Not_Virtual()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new Proxy<Foo>()
                .InterceptMethod(f => f.Return())
                .Save();
            });
        }

        [Fact]
        public void Should_Throw_If_Intercepted_Property_Is_Not_Virtual()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new Proxy<Foo>()
                .InterceptSetter(f => f.Description)
                .Save();
            });
        }
    }
}
