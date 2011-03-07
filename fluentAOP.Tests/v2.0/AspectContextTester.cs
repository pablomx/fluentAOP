using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAop.Tests.v2
{
    public class AspectContextTester
	{
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

            fAop.DescribeAspect("foo")
            .Before()
            .Do(() =>
            {
                ack = true;

            }).End();

            var foo = fAop.Weave(target, With.Aspect("foo"));

            Assert.False(ack);
            foo.Go();
            Assert.True(ack);
        }

        [Fact]
        public void Should_create_aspects_fluently()
        {
            bool ack = false;
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before()
            .Do(() =>
            {
                ack = true;

            }).End();

            var foo = fAop.Weave<Foo>();
            Assert.False(ack);
            foo.Go();
            Assert.True(ack);
        }

        [Fact]
        public void Should_intercept_specific_methods()
        {
            bool ack = false;
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before<IFoo>( f=>f.OverloadedGo(default(string)) )
            .Do(() =>
            {
                ack = true;

            }).End();
            
            var foo = fAop.Weave<IFoo>(new Foo()); //NOTE: If type is Foo it does not go through the proxy

            Assert.False(ack);
            foo.OverloadedGo(1);
            Assert.False(ack);

            Assert.False(ack);
            foo.OverloadedGo("");
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
        public void Should_pass_specific_arguments_using_their_names_and_types()
        {
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before<IFoo>(f => f.OverloadedGo(default(string)))
            //.Do<int,string>(Test)
            .Do((string a, string b) =>
            {
                Assert.Equal(a, "ack");
                Assert.Null(b);
            })            
            .End();

            var foo = fAop.Weave<IFoo>(new Foo());
            foo.OverloadedGo("ack");
        }


        [Fact]
        public void Should_pass_special_argument_this()
        {
            IFoo foo = new Foo();
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before<IFoo>(f => f.Go())
            .Do((IFoo @this, IFoo whatever) =>
            {
                Assert.Equal(foo, @this);
                Assert.Null(whatever);
            })
            .End();

            fAop.Weave<IFoo>(foo).Go();
        }

        [Fact]
        public void Should_pass_special_argument_args()
        {
            IFoo foo = new Foo();
            var fAop = new AspectContext();

            fAop.DescribeAspect("foo")
            .Before<IFoo>(f => f.OverloadedGo(default(int)))
            .Do((object[] @args) =>
            {
                Assert.Equal(@args[0],"ack");
            })
            .End();

            fAop.Weave<IFoo>(foo).OverloadedGo("ack");
        }

	}
}
