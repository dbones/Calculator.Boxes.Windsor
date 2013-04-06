namespace Calc.Commands
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Core;
    using Identifiers;

    /// <summary>
    /// simple memory extension to the calculator
    /// </summary>
    public class CalculatorMemoryCommand : ICalculatorCommand, ITransientDependency
    {
        private readonly Dictionary<string, decimal> _memory = new Dictionary<string, decimal>();

        public string Execute(string input, decimal currentValue)
        {
            var lowercaseInput = input.ToLower();

            //add location val i.e. add temp1 500.99
            if (lowercaseInput.StartsWith("add"))
            {
                var strings = lowercaseInput.Split(' ');
                var location = strings[1];
                var value = decimal.Parse(strings[2]);
                if (_memory.ContainsKey(location))
                {
                    _memory[location] = value;
                }
                else
                {
                    _memory.Add(location, value);
                }
                return "";
            }
            //remove location i.e. remove temp1
            if (lowercaseInput.StartsWith("remove"))
            {
                var strings = lowercaseInput.Split(' ');
                var location = strings[1];
                if (_memory.ContainsKey(location))
                {
                    _memory.Remove(location);
                }
                return "";
            }

            //mem location i.e. mem temp1
            const string expression = @"mem \w*";
            var exp = new Regex(expression);

            return exp.Replace(lowercaseInput,
                match =>
                {
                    var strings = match.ToString().Split(' ');
                    var location = strings[1];
                    return _memory[location].ToString(CultureInfo.InvariantCulture);
                });
        }
    }
}
