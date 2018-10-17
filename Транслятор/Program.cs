using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Транслятор {

    internal class Program {

        public static int Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("No arguments specified!");
                return -1;
            }
            try {
                var t = new Translator("test.bin");
                t.Translate(args[0].RemoveWhiteSpaces());
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