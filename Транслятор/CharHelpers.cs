using System;

namespace Транслятор {

    /// <summary>
    /// Priority of the operators
    /// </summary>
    public enum Priority {
        Low = 0,
        Normal,
        High,
        Extreme
    }

    public static class CharHelpers {

        /// <summary>
        /// If character is number returns true 
        /// </summary>
        public static bool IsNumber(this char c) => int.TryParse(c.ToString(), out int i);

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
                    return true;
                default:
                    return false;
            }
        }

    }

}