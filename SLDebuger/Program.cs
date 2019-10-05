using System;
using System.Collections.Generic;
using System.IO;
using SLProject.SLCompilerLib;

namespace SLProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Compiler compiler = new Compiler();

            string code;
            string fname;
            string[] com = new string[0];
            bool debug = true;

            while(true){
                if (!File.Exists("test.txt"))
                {
                    Console.WriteLine("Fichier à compiler :");
                    fname = Console.ReadLine();
                    if (!File.Exists(fname))
                    {
                        Console.WriteLine("Ce fichier n'existe pas !");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Fichier test détecté !");
                    fname = "test.txt";
                }

                code = File.ReadAllText(fname);

                if(com.Length > 1 && com[0] == "$"){
                    switch(com[1]){
                        case "debug":
                            if(com[2] == "0"){
                                debug = false;
                                Console.WriteLine("$ debug = false");
                            }
                            else if(com[2] == "1"){
                                debug = true;
                                Console.WriteLine("$ debug = true");
                            }
                            else
                                Console.WriteLine($"OPTION \"debug\" doesn't accept this arg(\"{com[2]}\") !");
                            break;
                        case "quit":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine($"OPTION \"{com[1]}\" doesn't exist !");
                            break;

                    }
                }else{
                    try{
                        Console.WriteLine("Compilation en cours...");
                        compiler.Compile(code, debug);
                    }catch(Exception e){
                        Console.WriteLine("Une erreur est survenue :\n\t"+e.Message);
                    }
                    Console.WriteLine();
                }
                Console.ReadLine();
            }
        }
    }
}
