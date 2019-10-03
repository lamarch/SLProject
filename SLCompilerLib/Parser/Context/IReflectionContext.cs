using System;

namespace SLProject.SLCompilerLib.Parser{
    public interface IReflectionContext
    {
        double ResolveVariable(string name);
        double ResolveFunction(string name, params object[] pm);
        void SetVariable(string name, double value);
        void DelVariable(string name);
    }
}