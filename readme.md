# fluentAOP

An AOP _(Aspect-Oriented Programming)_ library that allows to implement aspects utilizing a fluent API. fluentAOP is primarily designed to simplify the adoption and use of AOP in .NET. It does not require XML files, attributes or any other kind of configuration. In contrast to most AOP implementations, its interception semantics exclusively rely on strongly-typed method definitions and a fluent API.

[Read more.](http://fluentaop.codeplex.com)

## AOP in one line of code
_An example is worth a thousand words..._

	var proxy = new Proxy<Foo>()
	   .Target( foo )
	   .InterceptMethod ( f => f.Go() )
	   .OnBefore( ()=> Console.Write("Hello World!") )
	   .Save();

	// Note: line indented to improve readability
	// Result: Go() method is intercepted and every time it's called a "Hello World!" message is previously printed.