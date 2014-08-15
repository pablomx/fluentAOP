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
    public class PropertyInterceptionTester
    {
        [Fact]
        public void Can_Intercept_Getter()
        {
            var ack = 0;
            var foo = new Foo();
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptGetter(f => f.Name)
                .OnBefore(() => { Assert.False(foo.WasExecuted); ack++; })
                .Save();

            Assert.Equal("foo", proxy.Name);
            Assert.True(foo.WasExecuted);
            proxy.Name = string.Empty; // setter shouldn't be intercepted
            Assert.Equal(1, ack);
        }

        [Fact]
        public void Can_Intercept_Setter()
        {
            var ack = 0;
            var foo = new Foo();
            var proxy = new Proxy<IFoo>()
                .InterceptSetter(f => f.Name)
                .OnBefore(() => { Assert.False(foo.WasExecuted); ack++; })
                .OnInvoke(mi => mi.Method.Invoke(foo, mi.Arguments))
                .Save();

            proxy.Name = string.Empty; // intercepted
            Assert.Equal(string.Empty, foo.Name); // getter shouldn't be intercepted
            Assert.True(foo.WasExecuted);
            Assert.Equal(1, ack);
        }

        [Fact]
        public void Can_Intercept_All_Setters()
        {
            var ack = 0;
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptAllSetters()
                .OnFinally(() => { ack++; })
                .Save();

            proxy.Name = string.Empty;
            proxy.Description = string.Empty;
            var name = proxy.Name; // not intercepted

            Assert.Equal(2, ack);
        }

        [Fact]
        public void Can_Intercept_All_Getters()
        {
            var ack = 0;
            var foo = new Foo();
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptAllGetters()
                .OnFinally(() => { ack++; })
                .Save();

            var name = proxy.Name;
            var description = proxy.Description;
            proxy.Name = string.Empty; // not intercepted

            Assert.Equal(2, ack);
        }

    }
}
