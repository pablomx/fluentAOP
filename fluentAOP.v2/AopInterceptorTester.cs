//using System;
//using Xunit;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace FluentAop.Tests.v2
//{
//    public class AopInterceptorTester
//    {
//        [Fact]
//        public void Should_intercept_method_execution()
//        {
//            var target = new Foo();
//            var pointcut = new Pointcut(mi => true);
//            var advice = new Advice(() => Assert.False(target.WasExecuted));
//            var aspect = new Aspect();
//            aspect.Append(AdviceType.Before, pointcut, advice);

//            var interceptor = new AopInterceptor(new List<Aspect> { aspect });

//            var factory = new Proxi.ProxyFactory();
//            var foo = factory.Create<IFoo>(target, interceptor);
//            foo.Go();

//            Assert.True(target.WasExecuted);
//        }
//    }
//}
