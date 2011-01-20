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
    public class MethodInterceptionTester
    {
        [Fact]
        public void Can_Callback_OnBefore()
        {
            var foo = new Foo();
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptMethod(f => f.Go())
                .OnBefore(() => Assert.False(foo.WasExecuted))
                .Save();

            proxy.Go();
            Assert.True(foo.WasExecuted);
        }

        [Fact]
        public void Can_Callback_OnAfter()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Intercept(f => f.Go())
                .OnInvoke(mi => foo.Go())
                .OnAfter(() => Assert.True(foo.WasExecuted))
                .Save();

            Assert.False(foo.WasExecuted);
            proxy.Go();
        }

        [Fact]
        public void Can_Callback_OnCatch_And_OnFinally()
        {
            var ack = 0;
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Intercept(f => f.Fail())
                .OnInvoke(mi => foo.Fail())
                .OnCatch(e => Assert.IsAssignableFrom<InvalidOperationException>(e))
                .OnFinally(() => ack++)
                .Save();

            proxy.Fail();
            Assert.Equal(1, ack);
        }

        [Fact]
        public void Can_Callback_OnBefore_OnFinally_And_OnAfter()
        {
            var ack = 0;
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Intercept(f => f.Go())
                .OnBefore(() => { ack++; Assert.False(foo.WasExecuted); })
                .OnInvoke(mi => foo.Go())
                .OnFinally(() => ack++)
                .OnAfter(() => { ack++; Assert.True(foo.WasExecuted); })
                .Save();

            proxy.Go();
            Assert.Equal(3, ack);
        }

        [Fact]
        public void Can_Callback_OnReturn_And_Replace_Return_Value()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Intercept(f => f.Return())
                .OnInvoke(mi => foo.Return())
                .OnReturn((mi, r) => r.Equals(1) ? -1 : -2)
                .Save();

            Assert.Equal(-1, proxy.Return());
            Assert.True(foo.WasExecuted);
        }

        [Fact]
        public void Can_Skip_Target()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .Intercept(f => f.Return())
                .OnInvoke(mi => -2) // returns back, target won't be called
                .Save();

            Assert.Equal(-2, proxy.Return());
            Assert.False(foo.WasExecuted);
        }

        [Fact]
        public void Can_Callback_OnFinally_If_Method_Throws()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Intercept(f => f.Fail())
                .OnInvoke(mi => foo.Fail())
                .OnFinally(() => Assert.True(foo.WasExecuted))
                .Save();

            Assert.Throws<InvalidOperationException>(() => proxy.Fail());
            Assert.True(foo.WasExecuted);
        }

        [Fact]
        public void Can_Infer_Target_Method_If_A_Target_Object_Exists()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .Intercept(f => f.Go())
                .OnBefore(() => Assert.False(foo.WasExecuted))
                //.OnInvoke(mi => foo.Go()) must be inferred from context
                .Save();

            proxy.Go();
            Assert.True(foo.WasExecuted);
        }

        [Fact]
        public void Shuold_Throw_If_Target_Does_Not_Exist()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                //.Target(foo)
                //.Intercept(f => f.Go())
                .OnBefore(() => Assert.False(foo.WasExecuted))
                .OnInvoke(mi => foo.Go());

            Assert.Throws<ProxyInitializationException>(() => proxy.Save());
        }

        [Fact]
        public void Should_Throw_If_Advices_Are_Not_Specified()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .Intercept(f => f.Go());
                //.OnBefore(() => Assert.False(foo.WasExecuted))
            
            Assert.Throws<ProxyInitializationException>(() => proxy.Save());
        }

        [Fact]
        public void Can_Intercept_All_Methods_Getters_And_Setters()
        {
            var ack = 0;
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptAll()
                .OnFinally(() => { ack++; })
                .Save();

            // intercepts all gettrers, setters and methods...
            proxy.Go();
            var name = proxy.Name;
            var description = proxy.Description;
            proxy.Name = string.Empty;
            var result = proxy.GenericGo<int>(-1);
            Assert.Equal(5, ack);
        }

        [Fact]
        public void Can_Implement_An_Interface()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Implement<IDisposable>()
                .Intercept(f => f.Go())
                .OnBefore(() => Assert.False(foo.WasExecuted))
                .OnInvoke(mi => foo.Go())
                .Save();

            Assert.True(typeof(IDisposable).IsAssignableFrom(proxy.GetType()));
        }

        [Fact]
        public void Can_Implement_Multiple_Interfaces()
        {
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Implement(typeof(IDisposable), typeof(ICloneable))
                .Intercept(f => f.Go())
                .OnBefore(() => Assert.False(foo.WasExecuted))
                .OnInvoke(mi => foo.Go())
                .Save();

            Assert.True(typeof(IDisposable).IsAssignableFrom(proxy.GetType()));
            Assert.True(typeof(ICloneable).IsAssignableFrom(proxy.GetType()));
        }

        [Fact]
        public void Can_Intercept_Generic_Methods()
        {
            var ack = 0;
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptMethod(f => f.GenericGo<int>(It.Any<int>()))
                    .OnBefore(() => { Assert.False(foo.WasExecuted); ack++; })
                .InterceptMethod(f => f.GenericGo<string>(It.Any<string>()))
                    .OnAfter(() => { ack++; })
                .Save();

            proxy.GenericGo<int>(1); // intercepted
            proxy.GenericGo<string>(string.Empty); // intercepted
            proxy.GenericGo<double>(2d); // not intercepted
            proxy.GenericGo<object>(It.Any<object>()); // not intercepted

            Assert.True(foo.WasExecuted);
            Assert.Equal(2, ack);
        }

        [Fact]
        public void Should_Throw_If_InterceptSetter_Is_Used_With_Methods()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new Proxy<IFoo>()
                    .InterceptSetter(f => f.Return())
                    .OnInvoke(mi => -1)
                    .Save();
            });
        }

        [Fact]
        public void Should_Throw_If_InterceptGetter_Is_Used_With_Methods()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new Proxy<IFoo>()
                    .InterceptGetter(f => f.Return())
                    .OnInvoke(mi => -1)
                    .Save();
            });
        }

        [Fact]
        public void Should_Throw_If_InterceptMethod_Is_Used_With_Properties()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new Proxy<IFoo>()
                    .InterceptMethod(f => f.Name)
                    .OnInvoke(mi => -1)
                    .Save();
            });
        }

        [Fact]
        public void Can_Intercept_Multiple_Members_Using_A_Single_Fluent_Instruction()
        {
            var ack = 0;
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptMethod(f => f.Go())
                    .OnBefore(() => Assert.Equal(0, ack))
                    .OnAfter(() => ack++)
                .InterceptGetter(f => f.Name)
                    .OnBefore(() => Assert.Equal(1, ack))
                    .OnAfter(() => ack++)
                .InterceptSetter(f => f.Description)
                    .OnBefore(() => Assert.Equal(2, ack))
                    .OnAfter(() => ack++)
                .Save();

            // intercepted
            proxy.Go();
            var name = proxy.Name;
            proxy.Description = string.Empty;

            // not intercpted
            proxy.Return();
            proxy.Name = string.Empty;
            var desc = proxy.Description;

            Assert.Equal(3, ack);
        }

        [Fact]
        public void Can_Intercept_Multiple_Members_Using_Many_Fluent_Instructions()
        {
            var ack = 0;
            var target = new Foo();
            var proxy = new Proxy<IFoo>();

            proxy.Target(target)
                .InterceptMethod(f => f.Go())
                .OnBefore(() => Assert.Equal(0, ack))
                .OnAfter(() => ack++);

            proxy
                .InterceptGetter(f => f.Name)
                .OnBefore(() => Assert.Equal(1, ack))
                .OnAfter(() => ack++);

            proxy
                .InterceptSetter(f => f.Description)
                .OnBefore(() => Assert.Equal(2, ack))
                .OnAfter(() => ack++);
            
            var foo = proxy.Save();

            // intercepted
            foo.Go();
            var name = foo.Name;
            foo.Description = string.Empty;

            // not intercepted
            foo.Return();
            foo.Name = string.Empty;
            var desc = foo.Description;

            Assert.Equal(3, ack);
        }

        [Fact]
        public void Can_Intercept_Overloaded_Methods()
        {
            var ack = false;
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptMethod(f => f.OverloadedGo(It.Any<string>()))
                .OnBefore(() => ack = true)
                .Save();

            proxy.OverloadedGo(-1); // not intercepted
            Assert.False(ack);
            proxy.OverloadedGo(string.Empty); // intercepted
            Assert.True(ack);
        }

        [Fact]
        public void Can_Intercept_Multiple_Methods_Using_A_Filter() 
        {
            var ack = 0;
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptWhere(mi => mi.Name.EndsWith("Go"))
                //.InterceptWhere(mi => mi.IsGenericMethod )
                .OnBefore(mi => ack++)
                .Save();

            // intercepted
            proxy.Go(); 
            proxy.GenericGo<int>(-1);
            proxy.OverloadedGo(-1);
            proxy.OverloadedGo(string.Empty);

            // not intercepted
            proxy.Name = string.Empty; 
            proxy.Return();

            Assert.Equal(4, ack);
        }

        [Fact]
        public void Can_Intercept_Many_Methods_With_One_Advice()
        {
            var ack = 0;
            var foo = new Foo(); 
            var proxy = new Proxy<IFoo>()
                .Target(foo)
                .InterceptMethods(
                    f => f.Return(), 
                    f => f.GenericGo<int>(It.Any<int>()),
                    f => f.Go())
                .OnBefore(mi => ack++)
                .Save();

            // intercepted
            proxy.Go();
            proxy.Return();
            proxy.GenericGo<int>(-1);

            // not intercepted
            proxy.Name = string.Empty;
            proxy.OverloadedGo(-1);
            proxy.OverloadedGo(string.Empty);

            Assert.Equal(3, ack);
        }

    }
}