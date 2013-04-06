namespace Calc.Core
{
    using System;
    using System.Collections.Generic;
    using Identifiers;

    public class Calculator : ITransientDependency
    {
        public Calculator(
            IFormulaParser formulaParser,
            IEnumerable<ICalculatorCommand> calculatorCommands)
        {
            FormulaParser = formulaParser;
            CalculatorCommands = calculatorCommands ?? new List<ICalculatorCommand>();
        }

        public IFormulaParser FormulaParser { get; private set; }
        public IEnumerable<ICalculatorCommand> CalculatorCommands { get; private set; }
        public Decimal CurrentValue { get; private set; }

        public void Execute(string input)
        {
            foreach (var calculatorCommand in CalculatorCommands)
            {
                input = calculatorCommand.Execute(input, CurrentValue);
            }
            if (input.Equals("")) return;
            CurrentValue = FormulaParser.Evaluate(input);
        }
    }
}
