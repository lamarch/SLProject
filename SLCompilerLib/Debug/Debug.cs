using System;

namespace SLProject.SLCompilerLib{
    static class Debug{
        public static bool on = true;
        public static ConsoleColor color = ConsoleColor.Green;
        public static void Write(string message){
            if(!on)
                return;
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = temp;
        }
    }
}