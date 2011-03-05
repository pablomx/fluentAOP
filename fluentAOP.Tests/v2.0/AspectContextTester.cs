using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAop.Tests.v2
{
    public class AspectContextTester
	{
        /*
         
         TODO: improve Weave()
         aspects can have different types
         
         fAop.Weave<Logging,Transactional,Security,IFoo>(foo);         
         fAop.Attach<Logging>().To<IFoo>(Foo); 

         fAop.Register<Logging>();
         
         * 
         * class Logging:Aspect
         * {
         *      public Logging()
         *      {
         *          Pointcut.AddRule(...
         *          Advice += Do();
         *          AdviceType = Before;
         *      }
         * }
         
         * 
         */


        [Fact]
        public void Should_register_aspects()
        {
            var fAop = new AspectContext();
            fAop.DescribeAspect("foo")
            .Before()
            .Do(() => { return; }).End();
            Assert.True(fAop.Contains("foo"));
        }

        [Fact]
        public void Should_weave_aspects() 
        {
            var ack = false;
            IFoo target = new Foo();
            var fAop = new AspectContext();

            fAop.DescribeAspect()
            .Before()
            .Do(() => 
            {
                ack = true;

            }).End();

            var foo = fAop.Weave(target, aspect => aspect.Name == "");

            Assert.False(ack);
            foo.Go();
            Assert.True(ack);
        }

        [Fact]
        public void Should_weave_aspects_fluently()
        {
            bool ack = false;
            var fAop = new AspectContext();
            
            var foo =
            fAop.DescribeAspect("foo")
            .Before()
            .Do(() =>
            {
                ack = true;

            }).Weave(new Foo());

            Assert.False(ack);
            foo.Go();
            Assert.True(ack);
        }

        [Fact]
        public void Should_intercept_specific_methods()
        {
            bool ack = false;
            var fAop = new AspectContext();

            var foo =
            fAop.DescribeAspect("foo")
            .Before<IFoo>( f=>f.Go() )
            .Do(() =>
            {
                ack = true;

            }).Weave(new Foo());

            Assert.False(ack);
            foo.OverloadedGo(1);
            Assert.False(ack);

            Assert.False(ack);
            foo.Go();
            Assert.True(ack);
        }


        //[Fact]
        //public void Should_register_aspects()
        //{
        //    var fAop = new AspectContext();

        //    fAop.DescribeAspect("Logging")
        //    .Before()
        //    .Do(() =>
        //    {
        //        Console.WriteLine("Before");

        //    }).End();

        //    Assert.True(fAop.Contains("Logging"));
        //}   


	}
}
