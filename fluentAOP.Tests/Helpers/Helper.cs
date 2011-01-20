using System;
using System.Linq;
using Xunit;
using FluentAop.Utility;
using OM = FluentAop.Tests.ObjectMother;

namespace FluentAop.Tests
{
    public interface IBasicInterface
    {
        void Go();
        int Return(string to);
    }
    
    public abstract class ItIsAbstract 
    {
        public abstract void Go();
        public abstract int Return(string to);
    }

    public class ItImplementsAnInterface : IBasicInterface
    {

        public void Go()
        {
        }

        public int Return(string to)
        {
            return -1;
        }
    }

    public class ItHasGenericMethods
    {
        public virtual T Go<T>(T t) 
        {
            return t;
        }
    }

    public class ItHasOverloadedMethods
    {
        public virtual void Go()
        {
        }
        
        public virtual int Go(string to) 
        {
            return -1;
        }

        public virtual bool Go(int id) 
        {
            return true;
        }
    }

    public class ItExtendsAnAbstractClass : ItIsAbstract
    {
        public override void Go()
        {
        }

        public override int Return(string to)
        {
            return -2;
        }
    }

    public class ItHasNonDefaultConstructor 
    {
        public ItHasNonDefaultConstructor(int id)
        {
        }

        public virtual int Go(string to)
        {
            return -1;
        }
    }

    public class ItHasVirtualProperties 
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }

    public class ItHasMethodWithRefParams
    {
        public virtual void ItHasARefParam(ref int param)
        {
            param = -1;
        }

        public virtual void ItHasAnOutParam(out int param)
        {
            param = -1;
        }
    }
 
}
