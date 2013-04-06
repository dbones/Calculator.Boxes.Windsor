namespace Calc.NCalcEngine
{
    using Core;
    using Identifiers;
    using NCalc;

    public class NCalcFormulaParser : IFormulaParser, ITransientDependency
    {
        public decimal Evaluate(string input)
        {
            var exp = new Expression(input);
            var temp = exp.Evaluate();
            return decimal.Parse(temp.ToString());
        }
    } 
}
