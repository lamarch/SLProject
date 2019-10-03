using System;

namespace SLProject.SLCompilerLib{
    static class Debug{
        public static bool on = true;
        public static ConsoleColor color = ConsoleColor.Green;

        static int TreeLevel = 0;

        public static void Write(string message){
            if(!on)
                return;
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = temp;
        }

        public static void StartTree(string name){
            WriteOffset("START -> " + name, TreeLevel++);
        }

        public static void EndTree(string name){
            WriteOffset("END -> " + name, --TreeLevel);
        }

        public static void WriteTree(string text){
            WriteOffset(text, TreeLevel);
        }

        static void WriteOffset(string text, int offset){
            if(!on)
                return;
            string offs = "";
            for (int i = 0; i < offset; i++){
                offs += "    ";
            }
            Write(offs + text);
        }

        public static void Init(){
            TreeLevel = 0;
        }
    }
}