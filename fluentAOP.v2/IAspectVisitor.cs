using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAop.Poc
{
    public interface IAspectVisitor
    {
        void Vist(Aspect aspect);
    }
}
