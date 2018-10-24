using System;

namespace Транслятор {

    internal class Program {

        public static int Main(string[] args) {
            if (args.Length < 2) {
                Console.WriteLine("No arguments specified!");
                return -1;
            }

            try {
                var t = new Translator(args[1]);
                t.Translate(args[0]);
                Console.WriteLine("~> Done!");
                Console.Read();
            }
            catch (Exception e) {
                Console.WriteLine("~> " + e.Message);
                return -1;
            }

            return 0;
        }

    }

}