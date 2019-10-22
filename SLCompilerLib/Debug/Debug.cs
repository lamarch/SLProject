using System;
using System.Collections.Generic;

namespace SLProject.SLCompilerLib{
    static class Debug{
        public static bool on = true;
        public static ConsoleColor color = ConsoleColor.Green;

        static Stack<string> branches = new Stack<string>();

        public static void Write(string message){
            if(!on)
                return;
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = temp;
        }

        public static void StartBranch(string name){
            branches.Push(name);
            WriteOffset("START -> " + branches.Peek(), branches.Count - 1);
        }

        public static void EndBranch(){
            WriteOffset("----\n", branches.Count - 1);
            branches.Pop();
        }

        public static void WriteBranch(string text){
            WriteOffset(text, branches.Count);
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
    }
}