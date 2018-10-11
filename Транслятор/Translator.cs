using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Транслятор {

    public static class Translator {

        #region Public Methods

        public static string Translate(string expression) {
            Stack<char> stack = new Stack<char>();
            int i = 0;
            char c = expression[i];
            var numberRegex = new Regex(@"\d+(\.\d+)*");
            var charRegex = new Regex(@"[\wa-zA-Z]");
            string output = string.Empty;
            while (i < expression.Length) {
                while (true) {
                    if (c.IsNumber()
                        || charRegex.IsMatch(c.ToString())) {
                        output += c;
                        if (i == expression.Length - 1)
                            goto Done;
                        c = expression[++i];
                    }
                    else {
                        output += " ";
                        break;
                    }
                }

                if (c.IsOperator()) {
                    while (stack.Count != 0
                           && stack.Peek().OperationPriority() >= c.OperationPriority()
                           && stack.Peek().OperationPriority() != Priority.Extreme) {
                        output += stack.Pop() + " ";
                    }

                    stack.Push(c);
                    if (i == expression.Length - 1)
                        goto Done;
                    c = expression[++i];
                }

                if (c == '(') {
                    stack.Push(c);
                    c = expression[++i];
                }

                if (c == ')') {
                    while (stack.Peek() != '(')
                        output += " " + stack.Pop();
                    stack.Pop();
                    if (i == expression.Length - 1)
                        goto Done;
                    c = expression[++i];
                }
            }

            Done:
            while (stack.Count > 0)
                output += " " + stack.Pop();
            return output;
        }

        #endregion

    }

}
