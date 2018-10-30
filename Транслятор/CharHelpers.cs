using System.Text.RegularExpressions;

namespace Транслятор {
    
    public static class CharHelpers {

        private static readonly Regex regex = new Regex(@"[a-zA-Z]");

        /// <summary>
        /// If character is number returns true 
        /// </summary>
        public static bool IsNumber(this char c) {
            int i;
            return int.TryParse(c.ToString(), out i);
        }

        /// <summary>
        /// If character is operator (+, -, /, *, etc.) returns true.
        /// </summary>
        public static bool IsOperator(this char c) {
            switch (c) {
                case '+':
                case '-':
                case '/':
                case '*':
                case '^':
                case '(':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsLetter(this char c) => regex.IsMatch(c.ToString());

    }

}