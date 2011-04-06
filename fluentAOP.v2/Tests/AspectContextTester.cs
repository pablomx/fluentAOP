using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using FluentAop.Tests;

namespace FluentAop.Poc
{
    public class AspectContextTester
	{
        [Fact]
        public void Should_register_aspects_fluently()
        {
            var fAop = new AspectContext();
            fAop.DescribeAspect("foo")
            .Before()
            .Do(() => 
            { 
                return;
            
            }).End();

            Assert.True(fAop.Contains("foo"));
        }

        [Fact]
        public void Should_weave_specific_aspects() 
        {
            var ack = false;
            IFoo target = new Foo();
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before()
            .Do(() =>
            {
                ack = true;

            }).End();

            var foo = fAop.Weaver
                .Attach("foo")
                .To<IFoo>(new Foo());

            Assert.False(ack);
            foo.Go();
            Assert.True(ack);
        }

        //[Fact]
        //public void Should_weave_all_existing_aspects_by_default()
        //{
        //    bool ack = false;
        //    var fAop = new AspectContext();

        //    fAop.DescribeAspect("foo")
        //    .Before()
        //    .Do(() =>
        //    {
        //        ack = true;

        //    }).End();

        //    var foo = fAop.Weave<Foo>(); // weaves all aspects by default
        //    Assert.False(ack);
        //    foo.Go();
        //    Assert.True(ack);
        //}

        [Fact]
        public void Should_intercept_specified_methods_only()
        {
            bool ack = false;
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before<IFoo>( f=>f.OverloadedGo( It.Param<string>() )) // method to be intercepted
            .Do(() =>
            {
                ack = true;

            }).End();
            
            var foo = fAop.Weaver
                .Attach("foo")
                .To<IFoo>(new Foo());            
            //NOTE: If type is Foo it does not go through the proxy

            Assert.False(ack);
            foo.OverloadedGo(1); //not intercepted
            Assert.False(ack);

            Assert.False(ack);
            foo.OverloadedGo(""); //intercepted
            Assert.True(ack);
        }


        [Fact]
        public void Should_weave_aspects_fluently() 
        {
            bool ack = false;
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before()
            .Do(() =>
            {
                ack = true;

            }).End();

            var foo = fAop.Weaver
                .Attach("foo")
                .To<IFoo>(new Foo());

            Assert.False(ack);
            foo.OverloadedGo(1);
            Assert.True(ack);
           
        }

        [Fact]
        public void Should_select_arguments_by_name_and_type()
        {
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before<IFoo>(f => f.OverloadedGo(It.Param<string>()))
            .Do((string s, string t) => // method definition: OverloadedGo(string s);
            {
                Assert.Equal(s, "ack"); // it should pass in arg 's'
                Assert.Null(t); // arg 't' does not exist so it should pass null
            })            
            .End();

            var foo = fAop.Weaver
                .Attach("foo")
                .To<IFoo>(new Foo());

            foo.OverloadedGo("ack");
        }


        [Fact]
        public void Should_pass_target_object()
        {
            IFoo foo = new Foo();
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before<IFoo>(f => f.Go())
            .Do((IFoo @this, IFoo whatever) =>
            {
                Assert.Equal(foo, @this); // it should pass target object
                Assert.Null(whatever); // whatever does not exist do it should pass null
            })
            .End();

            fAop.Weaver
                .Attach("foo")
                .To<IFoo>(foo)
                .Go();
        }

        [Fact]
        public void Should_pass_all_method_arguments_as_an_arrray_if_args_is_used()
        {
            IFoo foo = new Foo();
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before<IFoo>(f => f.OverloadedGo(It.Param<int>()))
            .Do((object[] @args) =>
            {
                Assert.Equal(@args[0],"ack"); // it should pass all method arguments
            })
            .End();

            fAop.Weaver
                .Attach("foo")
                .To<IFoo>(new Foo())
                .OverloadedGo("ack");
        }

        [Fact]
        public void Should_register_and_weave_existing_aspects()
        {
            var fAop = new AspectContext();
            fAop.RegisterAspect<FooAspect>();

            var foo = fAop.Weaver
                .Attach<FooAspect>()
                .To<Foo>();

            var param = new System.Text.StringBuilder("ack");
            foo.OverloadedGo(param);
            Assert.Equal("ack_intercepted", param.ToString());
        }

	}

    #region Helpers
    class FooAspect : Aspect
    {
        public override void Describe(IAspectBuilder describer)
        {
            describer
            .Before()
            .Do((System.Text.StringBuilder sb) =>
            {
                sb.Append("_intercepted");
                return;

            }).End();
        }
    }
    #endregion

}
