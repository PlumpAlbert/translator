using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace Транслятор {

    public class Translator : IDisposable {

        #region Private Members

        private BinaryWriter bWriter;

        #endregion

        #region Constructor

        /// <summary>
        /// Construct <see cref="Translator"/> object with the output in file, specified in path
        /// </summary>
        /// <param name="path">The path of the output file</param>
        public Translator(string path) {
            this.bWriter = new BinaryWriter(File.Create(path), Encoding.ASCII, false);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Translates the expression
        /// </summary>
        public void Translate(string expression) {
            Variable[] variables;
            var list = ReversePolishNotation(expression, out variables).Split(' ');
            // Writing variable table to the destination file
            var bf = new BinaryFormatter();
            bf.Serialize(this.bWriter.BaseStream, variables);

            // Writing code for interpreter
            var stack = new Stack<ushort>();
            foreach (var s in list) {
                if (s.IsOperation()) {
                    // Write code operation
                    this.bWriter.Write(s.EncodeOperation());
                    // Take first argument
                    var tmp = stack.Pop();
                    // If it's a binary function
                    if (s.IsBinary())
                        // Write left argument
                        this.bWriter.Write(stack.Pop());
                    // Write argument
                    this.bWriter.Write(tmp);
                    // Push index of right argument as a result
                    stack.Push(tmp);
                }
                else stack.Push(ushort.Parse(s));
            }
        }
        
        /// <summary>
        /// Transforms infix notation into the reversed Polish notation
        /// </summary>
        /// <param name="src">Expression to translate</param>
        /// <param name="varArray">Array, which will contain all of the variables/constants</param>
        /// <returns>Expression in reversed Polish notation</returns>
        private string ReversePolishNotation(string src, out Variable[] varArray) {
            // Preprocessing expression
            src = src.RemoveWhiteSpaces();
            src = src.Replace(',', '.');
            // Define variables
            var stack = new Stack<string>();
            char c;
            var buf = string.Empty;
            var numberRgx = new Regex(@"\d+(\.\d+)*");
            var varRgx = new Regex(@"^-{0,1}(\w+_*)*(\d+)*$");
            // Define output variables
            var varList = new List<Variable>();
            var output = string.Empty;
            ////////////////////////////////
            for (int i = 0; i < src.Length; i++) {
                // Take character from src string
                c = src[i];

                // Perform action, depending on type of this character
                // If it's a number
                if (c.IsNumber() || c == '.') {
                    // If buffer string is empty
                    // or a valid number
                    // or a valid variable 
                    if (string.IsNullOrEmpty(buf) 
                        || numberRgx.IsMatch(buf)
                        || varRgx.IsMatch(buf)) {
                        // Write it to buffer
                        buf += c;
                        continue;
                    }
                    throw new Exception("Wrong name of the variable");
                }

                // If it's a letter
                if (c.IsLetter()) {
                    if (string.IsNullOrEmpty(buf) 
                        || varRgx.IsMatch(buf)) {
                        buf += c;
                        continue;
                    }

                    throw new Exception("Wrong name of the variable");
                }

                // If the buf string is not empty
                if (!string.IsNullOrEmpty(buf)) {
                    // Check if it's an operation
                    if (buf.IsOperation())
                        stack.Push(buf);
                    else {
                        // Write index of the constant / variable
                        output += varList.Count + " ";
                        // Push variable in list
                        varList.Add(new Variable(buf));
                    }

                    buf = string.Empty;
                }

                switch (c) {
                    // If it's a opening bracket
                    case '(':
                        stack.Push("(");
                        continue;
                    // If it's a closing bracket
                    case ')': {
                        if (stack.Count > 0) {
                            while (stack.Peek() != "(") {
                                // Write operation to the output
                                output += stack.Pop() + " ";
                                if (stack.Count == 0)
                                    throw new Exception("Wrong bracket sequence");
                            }
                        }
                        else throw new Exception("Wrong bracket sequence");

                        stack.Pop();
                        if (stack.Count > 0 && !stack.Peek().IsBinary()) {
                            output += stack.Pop() + " ";
                        }
                        continue;
                    }
                }

                // If it's a operator
                if (c.IsOperator()) {
                    // If it's a minus
                    if (c == '-') {
                        // Check if it's a negative
                        if (i - 1 < 0 || src[i - 1].IsOperator()) {
                            buf += c;
                            continue;
                        }
                    }
                    if (stack.Count > 0) {
                        while (stack.Peek().IsPrefixFunction()
                               || stack.Peek().OperationPriority() > c.ToString().OperationPriority()
                               || stack.Peek().LeftAssociative()
                               && stack.Peek().OperationPriority() == c.ToString().OperationPriority()) {
                            if(string.IsNullOrEmpty(output))
                                throw new Exception("Error in the expression");
                            // Write operation to the output
                            output += stack.Pop() + " ";
                            if (stack.Count == 0) break;
                        }
                    }

                    stack.Push(c.ToString());
                    continue;
                }

                throw new Exception("Error parsing the expression");
            }

            if (!string.IsNullOrEmpty(buf)) {
                // Write index of the constant / variable
                output += varList.Count + " ";
                // Push variable in list
                varList.Add(new Variable(buf));
            }

            while (stack.Count > 0) {
                if (stack.Peek() == "(")
                    throw new Exception("Wrong bracket sequence");
                // Write operation to the output
                output += stack.Pop() + " ";
            }

            varArray = varList.ToArray();
            varList.Clear();
            stack.Clear();
            return output.TrimEnd();
        }


        #endregion

        public void Dispose() {
            this.bWriter?.Close();
            this.bWriter?.Dispose();
        }

    }

}
