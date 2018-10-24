using System;
using System.Text.RegularExpressions;

namespace Транслятор {

    public static class StringHelpers {

        /// <summary>
        /// Removes whitespaces in current string
        /// </summary>
        public static string RemoveWhiteSpaces(this string s) {
            string res = string.Empty;
            foreach (var c in s) {
                if (c != ' ')
                    res += c;
            }

            return res;
        }

        /// <summary>
        /// Checks if the string is a prefix function (sin, cos, etc...)
        /// </summary>
        public static bool IsPrefixFunction(this string s) {
            switch (s) {
                case "sin":
                case "cos":
                case "tan":
                case "ctg":
                case "sqrt":
                case "ln":
                    return true;
                default: return false;
            }
        }

        /// <summary>
        /// Encodes operation
        /// </summary>
        /// <param name="s">Operation to encode</param>
        /// <returns>Code of the operation (byte)</returns>
        public static byte EncodeOperation(this string s) {
            switch (s) {
                default: return 0;
                case "+": return 1;
                case "-": return 2;
                case "/": return 3;
                case "*": return 4;
                case "^": return 5;
                case "sin": return 6;
                case "cos": return 7;
                case "tan": return 8;
                case "ctg": return 9;
                case "sqrt": return 10;
                case "ln": return 11;
            }
        }

        /// <summary>
        /// Returns <see cref="Priority"/> for this operation
        /// </summary>
        public static Priority OperationPriority(this string s) {
            switch (s) {
                case "^":
                    return Priority.Extreme;
                case "*":
                case "/":
                    return Priority.High;
                case "+":
                case "-":
                    return Priority.Normal;
                case "(":
                case ")":
                    return Priority.Low;
            }

            throw new ArgumentException("String is not a operator");
        }

        /// <summary>
        /// Checks if the current string is operation
        /// </summary>
        public static bool IsOperation(this string s) {
            switch (s) {
                case "sin":
                case "cos":
                case "tan":
                case "ctg":
                case "sqrt":
                case "ln":
                case "+":
                case "-":
                case "/":
                case "*":
                case "^":
                    return true;
                
                default: return false;
            }
        }

        /// <summary>
        /// Checks if the operation is left associative
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool LeftAssociative(this string s) {
            switch (s) {
                case "^":
                    return false;
                default: return true;
            }
        }

        public static bool IsBinary(this string s) {
            switch (s) {
                case "+":
                case "-":
                case "/":
                case "*":
                case "^":
                    return true;
                default:
                    return false;
            }
        }
    }

}