using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Reflection;

namespace SLProject.SLCompilerLib.Parser{
    public class ReflectionContext : IReflectionContext{
        object[] contexts;
        Dictionary<string, double> variables = new Dictionary<string, double>();
        public ReflectionContext(params object[] contexts){
            this.contexts = contexts;
        }

        public double ResolveVariable(string name){
            PropertyInfo pi = null;
            
            foreach (object ctx in contexts){
                pi = ctx.GetType().GetProperty(name);
                if(pi != null)
                    return (double)pi.GetValue(ctx);
            }
            
            if(variables.ContainsKey(name))
                return variables[name];
            else
                Parser.ThrowException(Parser.ExceptionType.UnknownIdentifier, "No variable named \""+name+"\" !", "REFLECTIONCONTEXT_resolvevariable_27");
            return 0;
        }

        public void SetVariable(string name, double value){
            if(variables.ContainsKey(name))
                variables[name] = value;
            else
                variables.Add(name, value);
        }

        public void DelVariable(string name){
            if(variables.ContainsKey(name))
                variables.Remove(name);
        }

        public double ResolveFunction(string name, params object[] pm){
            return 0;
        }

    }
}