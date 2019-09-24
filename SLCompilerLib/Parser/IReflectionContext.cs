using System;

namespace SLProject.SLCompilerLib.Parser{
    public interface IReflectionContext
    {
        double ResolveVariable(string name);
    }
}