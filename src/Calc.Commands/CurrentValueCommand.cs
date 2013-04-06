namespace Calc.Commands
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Core;
    using Identifiers;

    /// <summary>
    /// inputs the current value into the formula
    /// </summary>
    public class CurrentValueCommand : ICalculatorCommand, ITransientDependency
    {
        public string Execute(string input, decimal currentValue)
        {
            var lowercaseInput = input.ToLower();

            const string expression = "cur";
            var exp = new Regex(expression);

            return exp.Replace(lowercaseInput, match => currentValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}