using System;

namespace Транслятор {

    internal class Program {

        public static int Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("No arguments specified!");
                return -1;
            }
            try {
                Console.WriteLine(Translator.Translate(args[0].RemoveWhiteSpaces()));
                Console.Read();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return -1;
            }
            
            return 0;
        }

    }

}