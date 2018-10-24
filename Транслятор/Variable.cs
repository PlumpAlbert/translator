using System;
using System.Globalization;

namespace Транслятор {

    [Serializable]
    public class Variable {

        /// <summary>
        /// The name of the variable (blank if const)
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The value of the variable (blank if not const)
        /// </summary>
        public double Value { get; set; }


        public Variable(string name) {
            double result;
            // If it's a const
            if (double.TryParse(name, NumberStyles.Any, CultureInfo.InvariantCulture, out result)) {
                this.Name = string.Empty;
                this.Value = result;
                return;
            }

            // Otherwise
            this.Name = name;
            this.Value = double.NaN;
        }

    }

}