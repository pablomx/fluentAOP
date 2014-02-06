using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Reflection;
using Proxi;

namespace FluentAop.Poc
{
    public interface IAdvice
    {
        void Run(IMethodInvocation mi);
    }
}
