using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace SLProject.SLCompilerLib.Parser{
    public class ReflectionContext : IReflectionContext{
        object context;
        Dictionary<string, double> variables = new Dictionary<string, double>();
        public ReflectionContext(object context){
            this.context = context;
        }

        public ReflectionContext(object context, string fname_varLoader) : this(context){
            variables = LoadVariablesFromFile(fname_varLoader);
        }

        public double ResolveVariable(string name){
            var prop = context.GetType().GetProperty(name);
            if(prop != null)
                return (double)prop.GetValue(context);
            else if(variables.ContainsKey(name))
                return variables[name];
            else
                throw new Exception($"Unknown variable named \"{name}\" !");
        }

        static Dictionary<string, double> LoadVariablesFromFile(string fname){
            Dictionary<string, double> vars = new Dictionary<string, double>();
            List<string> errors = new List<string>();
            int index=0;
            string[] lines = File.ReadAllLines(fname);
            foreach(string line in lines){

                index++;
                string[] args = line.Split('=');
                if(args.Length != 2){
                    errors.Add($"Unavailable line<{index}> !");
                    continue;
                }

                string name = args[0];
                string val = args[1];
                double dval = 0;

                if(!double.TryParse(val, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out dval)){
                    errors.Add($"Unavailable value({val}) in var \"{name}\" at line<{index}>");
                    continue;                    
                }

                vars.Add(name, dval);
            }

            foreach(var err in errors){
                Console.WriteLine("Error on loading variables file : " + err);
            }

            return vars;
        }

    }
}