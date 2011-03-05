using System;
using Xunit;

namespace FluentAop.Tests.v11
{

    public class AspectContextTester
	{
        [Fact]
        public void Should_create_aspects() 
        {
            var fAop = new AspectContext();

            //fAop.Describe<Aspect>("Logging")

            fAop.CreateAspect("Logging")
                .Before<IFoo>(foo=>foo.Go(), foo=>foo.Return())
                .Do( mi =>
                {
                    Console.WriteLine("Before execution");
                
                })
                .AfterThrowing<IFoo>(foo => foo.OverloadedGo(default(string)))
                .Do<IFoo, string, int>((@this, arg1, arg2) =>
                {
                    //throw ex;

                })
                .AfterThrowing<IFoo>(foo=>foo.Go(), foo=>foo.Return())
                .Do(ex =>
                {
                    throw ex;

                }).End();
            
            //Assert.IsType<Aspect<IFoo>>(aspect);
        }   

        void Syntax() 
        {/*
            var fAop = new AspectContext();

            fAop.CreateAspect<IFoo>(" foo logging ")
                .Before(method => method.Go())
                    .Do(()=> Console.Write("before..."))
                    .Do(()=> Console.Write("before..."))
                    .Do(() => Console.Write("before..."))
                .After(Method.Where<IFoo>())
                    .Do(() => Console.Write(""))
                //.Finally()
                //    .Do(() => Console.Write(""))
                //.BeforeReturn() //onreturn
                //.BeforeThrow() //oncatch
                //.BeforeInvoke()
                .End(); // Weave(foo);
            
            var foo = fAop.Weave(new Foo());

            var builder = fAop.Builder; // fluent api
            var weaver = fAop.Weaver; // Proxi dependencies
            var factory = fAop.Factory; // create singletons

            //syntax 1
            fAop.FromType<IFoo>()
                   .Intercept(pointcut => {
                        pointcut.Go();
                   })
                   .With(advice => { 
                       advice.OnBefore(()=> Console.WriteLine("Hi"));
                   });

            //syntax 2
            fAop.CreateAspect<IFoo>("logging")
                    .Intercept(f=>f.Go())
                    .With()
                    .OnBefore(()=> Console.Write());
                    //.Weave() vs .Save() ??

            //syntax 3
            var aspect = new Aspect<IFoo>("name");
            aspect.Pointcuts = new PointcutCollection();
            aspect.Advices = new AdviceCollection();
            fAop.Register(aspect);

            //other examples
            var foo = new Foo();
            fAop.Weave<IFoo>(foo);
            fAop.Weave<IFoo>("logging", foo);
            fAop.AttachAspect<IFoo>().To(foo);
            Aspect<IFoo> aspect = fAop.GetAspect<IFoo>();
            var newfoo = aspect.Attach(foo);
            foo = (newfoo as Aspect<>).Detach();

            var proxy = new AspectWeaver<IFoo>()
                .Intercept(f=>f.Go())
                .OnBefore(()=> Console.Write(""))
                .Save();

            .After(method => method.Go(default(string), default(int)))
			.Do((string $2) => Console.Write(arg1))
            .Do((int $1) => Console.Write(arg2))

		var context = new Context();
		
		context.
			.Before(method => method.Go())
			.Do(()=>
			{
				DoSomething();
			})
			.After(method => method.Go(default(string)))
			.Do((string arg1) => Console.Write(arg1))
			
			.AfterReturning
			.AfterThrowing
			.BeforeInvoking
			
		var pointcut = new Pointcut<IFoo>("");
		pointcut.Append(foo => foo.Go())
			.Append(foo => foo.Goo())
			...
			
		.Before(pointcut)
		.Do(
        
        */}
	}
}
