## Project Description

An Aspect-Oriented Programming library that allows to implement aspects utilizing a fluent API. fluentAOP is primarily designed to simplify the adoption and use of AOP in .NET. It does not require XML files, attributes or any other kind of configuration. In contrast to most AOP implementations, its interception semantics exclusively rely on strongly-typed method definitions and a fluent API.

## AOP in one line of code
_An example is worth a thousand words..._

```c#
// Note: line indented to improve readability
var foo = new Proxy<Foo>()
   .Target( new Foo() )
   .InterceptMethod ( f => f.Go() )
   .OnBefore(()=> Console.Write(“Hello World!”) )
   .Save();

// Result: every time Go() is called a “Hello World!” message is previously printed.
foo.Go(); 
```

Take a look at the QuickStart (see below) for more examples.

## fluentAOP: A simple way to introduce AOP in .NET applications

The aim of this project is not to offer the most complete AOP implementation or the richest interception semantics. fluentAOP is an attempt to simplify the adoption and use of AOP in .NET applications. 

In order to achieve this goal fluentAOP offers:

* **Strongly-typed syntax.** Interception semantics are based on strongly-typed method definitions, which permit to develop aspects taking advantage of features like: auto-completion, refactoring, compile-time errors, etc.

* **No configuration.** fluentAOP tries to simplify AOP implementation by favoring convention over configuration. As a result, no configuration of any kind is ever required to build aspects. However, some conventions take place to make this possible.

* **Minimum learning-curve.** In order to get started with fluentAOP no previous experience with this or any other AOP implementation is required. No need to know or understand AOP terminology, which is not always very intuitive. By looking at some examples developers can figure out how to intercept calls and modularize their own applications.

* **Methods as first-class elements.** Utilizing dynamic proxies to implement AOP typically results in having to model aspects as interceptors. Such interceptors are commonly associated with objects no with methods. Therefore, the developer is responsible for providing the logic to break down object interception into method interception. fluentAOP alleviates this disparity by providing a true method-oriented approach.

* **Non-intrusive.** Using fluentAOP can result in a clean and well-modularized implementation of AOP. Advices are modeled as POCOs. They do not need any type of decoration, configuration or type hierarchy to compose aspects. Indeed, any advice implementation can be easily reused out of the scope of fluentAOP.

## Weaving

fluentAOP does not depend on any compile-time process to introduce aspects. Rather, it relies on Proxi (_Proxy Interface_) to perform runtime IL-weaving. Proxi is a light-weight library that makes possible to generate runtime-built proxies (also known as dynamic proxies) using different providers. To know more about the Proxi project visit its [website](https://github.com/pablomx/proxi).

## Proxies

**What types of proxies does fluentAOP support?**

* **Interface Proxy.** It implements an interface as its base type and utilizes composition to delegate calls to its target object. Any method or property exposed by the interface can be intercepted. 

* **Class Proxy.** It extends a class and overrides its virtual methods and properties to provide interception. Any method or property exposed as virtual in the extended type can be intercepted.

* **Non-Target Proxy.** It does not require a target object and its base type can be abstract (class or interface). Any method or property exposed by its base type can be individually implemented on runtime.

## Advices

**What types of advices does fluentAOP support?**

* **OnBefore.** It always runs before the target method execution.

* **OnInvoke.** It runs instead of the target method execution.

* **OnAfter.** It runs after the target method execution, but only if it does not throw.

* **OnCatch.** It runs after the target method execution, but only if it throws.

* **OnFinally.** It always runs after the target method execution. But OnAfter and OnCatch advices always run first if they exist.

* **OnReturn.** It runs right before the proxy method execution is over. OnFinally advice runs first if it exist.

## Limitations

Like any other AOP implementation, based on runtime-built proxies, fluentAOP only supports a subset of AOP:

* Interception has to be defined around method or property calls: on entry, on exit or on exception.

* Only virtual or interface members (methods or properties) can be intercepted.

## FAQ

**Does fluentAOP support property-based interception?**
Yes, property and method interception are supported. 

**Are non-target proxies supported?**
Yes, it is possible to build proxies without having to provide a target object.

**What do I need to utilize fluentAOP?**
* Download the latest release from this website.
* Select 'Add Reference...' on your Visual Studio project.
* Locate and select: fluentAOP.dll and Proxi.dll.

## QuickStart and Examples

* Advices
* OnBefore Advice
* OnAfter Advice
* OnCatch and OnFinally Advices
* OnInvoke Advice
* OnReturn Advice
* Implementing additional interfaces
* Intercepting multiple calls with a single advice
* Intercepting all calls with a single advice
* Differentiating between overloaded methods
* Stacking several methods on a single fluent instruction
* Utilizing predicates to select multiple methods
* Intercepting Properties I: Setters
* Intercepting Properties II: Getters
* Nested advices
* Non-target proxies (pending)
* Method wrappers (pending)
* Intercepting virtual members (pending)
* Changing the CIL provider (pending)

**1. Advices**

**1.1 OnBefore Advice**

It always runs before the target method execution.

```c# 
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptMethod(f => f.Go())
	.OnBefore(()=> Console.WriteLine("1,2,3..."))
	.Save();

foo.Go();

// Console Output:
// 1,2,3...
// Go!
```

**1.2 OnAfter Advice**

It runs after the target method execution, but only if it does not throw.

```c#
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptMethod(f => f.Go())
	.OnAfter(() => Console.WriteLine("--Excecution Succeded--"))
	.Save();

foo.Go();

// Console Output:
// Go!
// --Excecution Succeded--
```

**1.3 OnCatch and OnFinally Advices**

OnCatch advice runs after the target method execution, but only if it throws.
OnFinally advice runs after the target method execution. But OnAfter and OnCatch advices always run first if they exist.

```c#
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
```

**1.4 OnInvoke Advice**

It runs instead of the target method execution.

```c#
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptMethod(f => f.Throw())
	.OnInvoke(mi => Console.Write("--Replace Method Execution Here--")) // mi: MethodInvocation
	.Save();

foo.Throw();

// Notes: Throw() method does not thow anymore!!
// Console Output:
// --Replace Method Execution Here--
```

**1.5 OnReturn Advice**

It runs right before the proxy method execution is over. OnFinally advice runs first if it exist.

```c#
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptMethod(f => f.Null())
	.OnReturn((mi, r) => r ?? "--Replace Any Null Value Here--") //r: ReturnedValue
	.Save();

Console.WriteLine(foo.Null());

// Console Output:
// --Replace Any Null Value Here--
```c#

**2. Implementing additional interfaces**

```c#
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.Implement<IDisposable>()
	.InterceptMethod(f => f.Go())
	.OnBefore(() => Console.WriteLine("--OnBefore--"))
	.Save();
```


**3. Intercepting multiple calls with a single advice**

```c#
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptMethods(
		f => f.Go(),
		f => f.Throw(),
		f => f.Null())
	.OnBefore(() => Console.WriteLine("--OnBefore--"))
	.OnAfter(() => Console.WriteLine("--OnAfter--"))
	.Save();     
```

**4. Intercepting all calls with a single advice**

```c#
    var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptAll()
	.OnBefore(() => Console.WriteLine("--OnBefore--"))
	.OnAfter(() => Console.WriteLine("--OnAfter--"))
	.Save();

    foo.Go(); // Intercepted
    foo.Name = "foo"; // Intercepted
    var name = foo.Name; // Intercepted
```

**5. Differentiating between overloaded methods**

```c#
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptMethod(f => f.Overloaded(It.Any<string>()))
	.OnBefore(mi => mi.Arguments[0](0) = "--Intercepted--") // Replaces arg0
	.Save();

foo.Overloaded(" Hi! ");

// Notes: method was intercepted
// Console Output:
// --Intercepted--

foo.Overloaded( 123 );

// Notes: method was not intercepted
// Console Output:
// 123
```

**6. Stacking several methods on a single fluent instruction**

```c#
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
```

**7. Utilizing predicates to select multiple methods**

```c#
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptWhere(method => method.Name.EndsWith("loaded")) // Intercepts any method that ends with 'loaded'
	.OnBefore(() => Console.WriteLine("--OnBefore--"))
	.Save();

foo.Overloaded(" Hi! "); // Intercepted
foo.Go(); // Not Intercepted
```

**8. Intercepting Properties I: Setters**

```c#
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptSetter(f => f.Name)
	.OnBefore(() => Console.WriteLine("--Setting Property--"))
	.Save();

foo.Name = "foo"; // Intercepted
var name = foo.Name; // Not Intercepted
```

**9. Intercepting Properties II: Getters**

```c#
var foo = new Proxy<IFoo>()
	.Target(new Foo())
	.InterceptGetter(f => f.Name)
	.OnBefore(() => Console.WriteLine("--Getting Property--"))
	.Save();

foo.Name = "foo"; // Not Intercepted
var name = foo.Name; // Intercepted
```

**10. Nested advices**

```c#
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
```

_**Target Class**_ 
_The examples shown in this page are based on the following target class definition:_

```c#
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
```

_Non-target proxies (pending)_

_Method wrappers (pending)_

_Intercepting virtual members (pending)_

_Changing the CIL provider (pending)_
