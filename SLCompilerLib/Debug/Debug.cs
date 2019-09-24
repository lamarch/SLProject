using System;

namespace SLProject.SLCompilerLib{
    static class Debug{
        public static bool debug = true;
        public static ConsoleColor color = ConsoleColor.Green;
        public static void Write(string message){
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = temp;
        }
    }
}