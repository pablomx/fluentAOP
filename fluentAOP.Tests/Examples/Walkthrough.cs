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
using Xunit;
using FluentAop.Utility;

namespace FluentAop.Tests.Examples
{
    public class fluentAOP_Walkthrough
    {
        #region Advices
        public void Example_of_how_to_apply_OnBefore_advice()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptMethod(f => f.Go())
                .OnBefore(()=> Console.WriteLine("1,2,3..."))
                .Save();

            foo.Go();

            // Console Output:
            // 1,2,3...
            // Go!
        }

        public void Example_of_how_to_apply_OnAfter_advice()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptMethod(f => f.Go())
                .OnAfter(() => Console.WriteLine("--Excecution Succeded--"))
                .Save();

            foo.Go();
            
            // Console Output:
            // Go!
            // --Excecution Succeded--
        }

        public void Example_of_how_to_apply_OnCatch_and_OnFinally_advices()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptMethod(f => f.Throw())
                    .OnCatch(ex => { throw new InvalidOperationException("--Some Error Message Here--", ex); })
                    .OnFinally(() => Console.Write("--Dispose Resources Here--"))
                .Save();

            foo.Throw();
            
            //  Console Output:
            //  Example_of_how_to_apply_OnCatch_and_OnFinally_advices has failed:
            //  --Some Error Message Here--
            //  System.InvalidOperationException...
            //  --Dispose Resources Here--
        }

        public void Example_of_how_to_apply_OnInvoke_advice()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptMethod(f => f.Throw())
                .OnInvoke(mi => Console.Write("--Replace Method Execution Here--")) // mi: MethodInvocation
                .Save();

            foo.Throw();
            
            // Notes: Throw() method does not thow anymore!!
            // Console Output:
            // --Replace Method Execution Here--
        }

        public void Example_of_how_to_apply_OnReturn_advice()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptMethod(f => f.Null())
                .OnReturn((mi, r) => r ?? "--Replace Any Null Value Here--") //r: ReturnedValue
                .Save();

            Console.WriteLine(foo.Null());
            
            // Console Output:
            // --Replace Any Null Value Here--
        }
        #endregion

        public void Example_of_how_to_implement_interfaces()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .Implement<IDisposable>()
                .InterceptMethod(f => f.Go())
                .OnBefore(() => Console.WriteLine("--OnBefore--"))
                .Save();
        }

        public void Example_of_how_to_apply_same_advices_to_multiple_methods()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptMethods(
                    f => f.Go(),
                    f => f.Throw(),
                    f => f.Null())
                .OnBefore(() => Console.WriteLine("--OnBefore--"))
                .OnAfter(() => Console.WriteLine("--OnAfter--"))
                .Save();            
        }

        public void Example_of_how_to_intercept_overloaded_methods()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptMethod(f => f.Overloaded(It.Any<string>()))
                .OnBefore(mi => mi.Arguments[0] = "--Intercepted--") // Replaces arg0
                .Save();

            foo.Overloaded(" Hi! ");
            
            // Notes: method was intercepted
            // Console Output:
            // --Intercepted--

            foo.Overloaded( 123 );
            
            // Notes: method was not intercepted
            // Console Output:
            // 123
        }

        public void Example_of_how_to_specify_multiple_methods()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptMethod(f => f.Go())
                    // Go() Advices:
                    .OnBefore(() => Console.WriteLine("1,2,3...")) 
                    .OnAfter(() => Console.WriteLine("--Excecution Succeded--"))
                .InterceptMethod(f => f.Throw())
                    // Throw() Advices:
                    .OnCatch(ex => { throw new InvalidOperationException("--Some Error Message Here--", ex); })
                    .OnFinally(() => Console.Write("--Dispose Resources Here--"))
                .InterceptMethod(f => f.Null())
                    // Null() Advices:
                    .OnReturn((mi, r) => r ?? "--Replace Any Null Value Here--")
                .Save();

            // fluentAOP Convention:
            // Methods must be followed by one or more advices
            // Advices only intercept the closest previously defined method
        }

        public void Example_of_how_to_specify_a_method_selection_criteria()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptWhere(method => method.Name.EndsWith("loaded")) // Intercepts any method that ends with 'loaded'
                .OnBefore(() => Console.WriteLine("--OnBefore--"))
                .Save();

            foo.Overloaded(" Hi! "); // Intercepted
            foo.Go(); // Not Intercepted
        }

        public void Example_of_how_to_intercept_setters()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptSetter(f => f.Name)
                .OnBefore(() => Console.WriteLine("--Setting Property--"))
                .Save();

            foo.Name = "foo"; // Intercepted
            var name = foo.Name; // Not Intercepted
        }

        public void Example_of_how_to_intercept_getters()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptGetter(f => f.Name)
                .OnBefore(() => Console.WriteLine("--Getting Property--"))
                .Save();

            foo.Name = "foo"; // Not Intercepted
            var name = foo.Name; // Intercepted
        }

        public void Example_of_how_to_define_advices_using_a_nested_closure()
        {
            var foo = new Proxy<IFoo>()
                .Target(new Foo())
                .InterceptMethod(f => f.Go())
                .With(m => 
                {                
                    m.OnBefore(()=> Console.WriteLine("--OnBefore--"));
                    m.OnAfter(()=> Console.WriteLine("--OnAfter--"));

                }).Save();

            foo.Go();
            
            // Console Output:
            // --OnBefore--
            // Go!
            // --OnAfter--
        }
    }

    #region Helpers
    public class Foo : IFoo
    {
        public string Name { get; set; }

        public void Go()
        {
            Console.WriteLine("Go!");
        }

        public void Throw()
        {
            throw new Exception();
        }

        public string Null()
        {
            return null;
        }

        public void Overloaded(int i) 
        {
            Console.WriteLine(i);
        }
        
        public void Overloaded(string s) 
        {
            Console.WriteLine(s);
        }
    }

    public interface IFoo
    {
        string Name { get; set; }
        void Go();
        void Throw();
        string Null();
        void Overloaded(int i);
        void Overloaded(string s);
    }
    #endregion
}
