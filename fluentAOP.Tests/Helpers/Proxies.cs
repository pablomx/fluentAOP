using System;
using System.Linq;
using Xunit;
using FluentAop.Utility;
using OM = FluentAop.Tests.ObjectMother;

namespace FluentAop.Tests
{
    #region Proxies
    class LinFuProxy<T> : Proxy<T>
    {
        protected override Proxi.ProxyBehavior Behavior { get { return Proxi.ProxyBehavior.LinFu; } }
    }

    class CastleProxy<T> : Proxy<T>
    {
        protected override Proxi.ProxyBehavior Behavior { get { return Proxi.ProxyBehavior.Castle; } }
    }

    class SpringProxy<T> : Proxy<T>
    {
        protected override Proxi.ProxyBehavior Behavior { get { return Proxi.ProxyBehavior.Spring; } }
    }
    #endregion

    #region Interceptors
    public class SpringInterceptor : AopAlliance.Intercept.IMethodInterceptor
    {
        public object Invoke(AopAlliance.Intercept.IMethodInvocation info)
        {
            return -3;
            //return info.Method.Invoke(info.Target, info.Arguments);
        }
    }

    public class LinfuInterceptor : LinFu.DynamicProxy.IInterceptor
    {
        public object Intercept(LinFu.DynamicProxy.InvocationInfo info)
        {
            return -3;
            //return info.TargetMethod.Invoke(info.Target, info.Arguments);
        }
    }

    public class CastleInterceptor : Castle.DynamicProxy.IInterceptor
    {
        public void Intercept(Castle.DynamicProxy.IInvocation invocation)
        {
            invocation.ReturnValue = -3;
            //invocation.Proceed();
        }
    }
    #endregion

    public class Tester
    {

        [Fact]
        public void FluentAOP()
        {
            var proxy = new CastleProxy<ItExtendsAnAbstractClass>()
                //.Target(ObjectMother.Create<ItExtendsAnAbstractClass>())
                .Intercept(c => c.Return(It.Any<string>()))
                //.OnBefore(()=> Console.Write("Intercepted"))
                .OnInvoke(mi => -3)
                .Save();

            Assert.Equal(-3, proxy.Return(string.Empty));
        }

        [Fact]
        public void Castle()
        {
            var gen = new Castle.DynamicProxy.ProxyGenerator();
            var proxy = gen.CreateClassProxy<ItExtendsAnAbstractClass>(new CastleInterceptor());
            Assert.Equal(-3, proxy.Return(string.Empty));
        }

        [Fact]
        public void Linfu()
        {
            var factory = new LinFu.DynamicProxy.ProxyFactory();
            var proxy = factory.CreateProxy<ItExtendsAnAbstractClass>(new LinfuInterceptor());
            Assert.Equal(-3, proxy.Return(string.Empty));
        }

        [Fact]
        public void Spring()
        {
            var factory = new Spring.Aop.Framework.ProxyFactory();
            factory.Target = new ItExtendsAnAbstractClass();
            factory.AddAdvice(new SpringInterceptor());

            var proxy = (ItIsAbstract) factory.GetProxy();
            Assert.Equal(-3, proxy.Return(string.Empty));
        }

    }

}
