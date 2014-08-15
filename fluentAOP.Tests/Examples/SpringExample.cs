using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using AopAlliance.Intercept;
using Spring.Aop.Framework;
using Spring.Aop.Support;
using FluentAop.Utility;

namespace FluentAop.Tests.Examples
{
    #region Helpers
    public interface ICommand
    {
        void Execute();
        void DoExecute();
    }

    public class ServiceCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Execute()");
        }

        public void DoExecute()
        {
            Console.WriteLine("DoExecute()");
        }
    }

    #region Spring Declarative Style Approach
    /*
   <object id="consoleLoggingAroundAdvice"
            type="Spring.Examples.AopQuickStart.ConsoleLoggingAroundAdvice"/>
    <object id="myServiceObject" type="Spring.Aop.Framework.ProxyFactoryObject">
        <property name="target">
            <object id="myServiceObjectTarget"
                type="Spring.Examples.AopQuickStart.ServiceCommand"/>
        </property>
        <property name="interceptorNames">
            <list>
                <value>consoleLoggingAroundAdvice</value>
            </list>
        </property>
    </object>
      
     //Spring.NET AOP is preferred to the programmatic style
     ICommand command = (ICommand) ctx["myServiceObject"]; 
     command.Execute();
     // Examples taken from http://www.springframework.net/doc-latest/reference/html/aop-quickstart.html
     */
    #endregion

    #endregion
    
    public class ConsoleLoggingAroundAdvice : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            Console.WriteLine("Advice executing...");
            object returnValue = invocation.Proceed();
            Console.WriteLine("Advice executed");
            return returnValue;
        }
    }

    public class SpringExample
    {
        [Fact]
        public void Example_of_how_to_use_Spring_programmatic_interception()
        {
            ProxyFactory factory = new ProxyFactory(new ServiceCommand());
            factory.AddAdvice(new ConsoleLoggingAroundAdvice());
            ICommand command = (ICommand)factory.GetProxy();

            command.DoExecute();
            // Console output:
            // Advice executing...
            // DoExecute()
            // Advice executed
            
            command.Execute();
            // Console output:
            // Advice executing...
            // Execute()
            // Advice executed
        }

        [Fact]
        public void Example_of_how_to_use_Spring_programmatic_interception_with_pointcuts()
        {

            ProxyFactory factory = new ProxyFactory(new ServiceCommand());
            factory.AddAdvisor(new DefaultPointcutAdvisor(
                new SdkRegularExpressionMethodPointcut("DoExecute"),
                new ConsoleLoggingAroundAdvice()
                ));
            ICommand command = (ICommand) factory.GetProxy();
            
            command.DoExecute();
            // Console output:
            // Advice executing...
            // DoExecute()
            // Advice executed
            
            command.Execute();
            // Console output:
            // Execute()
        }

        [Fact]
        public void Example_of_how_to_use_fluentAOP()
        {
            var command = new Proxy<ICommand>()
                .Target(new ServiceCommand())
                .InterceptMethod(c => c.DoExecute())
                .OnBefore(()=> Console.WriteLine("Advice executing..."))
                .OnAfter(()=> Console.WriteLine("Advice executed"))
                .Save();

            command.DoExecute();
            // Console output:
            // Advice executing...
            // DoExecute()
            // Advice executed

            command.Execute();
            // Console output:
            // Execute()

            // Notes:
            // Refactoring does not break it
            // Auto-completition can be used to select/change members
        }

    }
}
