using System;
using System.Collections.Generic;
using SLProject.SLCompilerLib;

namespace SLProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> keywords = new List<string>();

            Compiler compiler = new Compiler(keywords);

            string rep;
            string[] com;
            bool debug = true;

            while(true){
                Console.WriteLine("Votre calcul :");
                rep = Console.ReadLine();
                
                com = rep.Split(' ');
                if(com.Length > 2 && com[0] == "$"){
                    switch(com[1]){
                        case "debug":
                            if(com[2] == "0")
                                debug = false;
                            else if(com[2] == "1")
                                debug = true;
                            else
                                Console.WriteLine($"OPTION \"debug\" doesn't accept this arg({com[2]}) !");
                            break;
                    }
                }else{
                    try{
                        Console.WriteLine("Calcul en cours...");
                        compiler.Compile(rep, debug);
                    }catch(Exception e){
                        Console.WriteLine("Une erreur est survenue :\n\t"+e.Message);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
