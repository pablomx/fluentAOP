using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAop.Tests.v2
{
    public interface IAspectVisitor
    {
        void Vist(Aspect aspect);
    }
}
