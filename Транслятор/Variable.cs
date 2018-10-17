using System;

namespace Транслятор {

    [Serializable]
    public class Variable {

        /// <summary>
        /// The name of the variable (blank if const)
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The value of the variable (blank if not const)
        /// </summary>
        public double Value { get; set; }


        public Variable(string name) {
            // If it's a const
            if (double.TryParse(name, out double result)) {
                this.Name = string.Empty;
                this.Value = result;
                return;
            }

            // Otherwise
            this.Name = name;
            this.Value = default(double);
        }
    }

}