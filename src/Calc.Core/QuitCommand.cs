namespace Calc.Core
{
    using Identifiers;

    /// <summary>
    /// quits the app
    /// </summary>
    public class QuitCommand : ICalculatorCommand, ITransientDependency
    {
        public string Execute(string input, decimal currentValue)
        {
            var lowercaseInput = input.ToLower();

            return lowercaseInput.Equals("q") ? "" : input;
        }
    }
}