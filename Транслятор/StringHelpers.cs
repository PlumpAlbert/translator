using System;

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
        /// Gets the last character of this string
        /// </summary>
        public static char GetLastChar(this string s) {
            if (!string.IsNullOrWhiteSpace(s))
                return s[s.Length - 1];
            return 'a';
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
            }
        }

        /// <summary>
        /// Returns <see cref="Priority"/> for this operation
        /// </summary>
        public static Priority OperationPriority(this string c) {
            switch (c) {
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
    }

}