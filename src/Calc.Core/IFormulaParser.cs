namespace Calc.Core
{
    /// <summary>
    /// will be responsible for actually calculating the formula
    /// </summary>
    public interface IFormulaParser
    {
        /// <summary>
        /// calculate the formula
        /// </summary>
        /// <param name="input">a string representation of an equation to evaluate</param>
        /// <returns>the output as a decimal</returns>
        decimal Evaluate(string input);
    }
}