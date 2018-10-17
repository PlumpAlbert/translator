using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace Транслятор {

    public class Translator {

        #region Private Members

        private BinaryWriter bWriter;

        #endregion

        #region Constructor

        /// <summary>
        /// Construct <see cref="Translator"/> object with the output in file, specified in path
        /// </summary>
        /// <param name="path">The path of the output file</param>
        public Translator(string path) {
            this.bWriter = new BinaryWriter(File.OpenWrite(path), Encoding.ASCII, false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Translates the expression
        /// </summary>
        public void Translate(string expression) {
            var stack = new Stack<string>();
            int i = 0;
            char c = expression[i];
            var numberRegex = new Regex(@"\d+(\.\d+)*");
            var charRegex = new Regex(@"[a-zA-Z]");
            string output = string.Empty;
            /* Addition */
            var varList = new List<Variable>();

            while (i < expression.Length) {
                if (c.IsOperator()) {
                    while (stack.Count != 0
                           && stack.Peek().OperationPriority() >= c.ToString().OperationPriority()
                           && stack.Peek().OperationPriority() != Priority.Extreme) {
                        this.bWriter.Write(stack.Pop());
                    }

                    stack.Push(c.ToString());
                    if (i == expression.Length - 1)
                        goto Done;
                    c = expression[++i];
                    continue;
                }

                if (c == '(') {
                    stack.Push(c.ToString());
                    c = expression[++i];
                    continue;
                }

                if (c == ')') {
                    while (stack.Peek() != "(")
                        this.bWriter.Write(stack.Pop());
                    stack.Pop();
                    if (i == expression.Length - 1)
                        goto Done;
                    c = expression[++i];
                    continue;
                }

                while (true) {
                    if (c.IsNumber()
                        || charRegex.IsMatch(c.ToString())) {
                        output += c;
                        if (i == expression.Length - 1) {
                            this.bWriter.Write((ushort) varList.Count);
                            varList.Add(new Variable(output));
                            goto Done;
                        }
                        c = expression[++i];
                    }
                    else {
                        this.bWriter.Write((ushort) varList.Count);
                        varList.Add(new Variable(output));
                        output = string.Empty;
                        break;
                    }
                }

            }

            Done:
            while (stack.Count > 0)
                this.bWriter.Write(stack.Pop());
            this.bWriter.Seek(0, SeekOrigin.Begin);
            var bf = new BinaryFormatter();
            bf.Serialize(this.bWriter.BaseStream, varList.ToArray());
            this.bWriter.Close();
            this.bWriter.Dispose();
        }

        #endregion

    }

}
