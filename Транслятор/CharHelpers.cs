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

        /// <summary>
        /// Returns <see cref="Priority"/> for this operation
        /// </summary>
        public static Priority OperationPriority(this char c) {
            switch (c) {
                case '^':
                    return Priority.Extreme;
                case '*':
                case '/':
                    return Priority.High;
                case '+':
                case '-':
                    return Priority.Normal;
                case '(':
                case ')':
                    return Priority.Low;
            }

            throw new ArgumentException("Character is not a operator");
        }

    }

}