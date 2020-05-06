using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
    internal class StringCalc
    {

        #region Regexes

        private readonly Regex _bracketsRegex = new Regex(@"\((\-?(?:\(\-?\d+(?:\,\d+)?(?:[E|e][+|-]?\d+)?\)|\d+(?:\,\d+)?(?:[E|e][+|-]?\d+)?)(?:[\+|\-|\*\/\^](?:\(\-?\d+(?:\,\d+)?(?:[E|e][+|-]?\d+)?\)|\-?\d+(?:\,\d+)?(?:[E|e][+|-]?\d+)?)){1,})\)", RegexOptions.Compiled);

        private readonly Regex _exponentiationRegex = new Regex(@"((?:\(\-?\d+(?:\,\d+)?\)|(?:^[+|-])?\d+(?:\,\d+)?)(?:[E|e][+|-]?\d+)?)(\^)((?:\(\-?\d+(?:\,\d+)?\)|\-?\d+(?:\,\d+)?)(?:[E|e][+|-]?\d+)?)", RegexOptions.Compiled | RegexOptions.RightToLeft);

        private readonly Regex _multiplicationOrDivisionRegex = new Regex(@"((?:\(\-?\d+(?:\,\d+)?\)|(?:^[+|-])?\d+(?:\,\d+)?)(?:[E|e][+|-]?\d+)?)([*|\/])((?:\(\-?\d+(?:\,\d+)?\)|\d+(?:\,\d+)?)(?:[E|e][+|-]?\d+)?)", RegexOptions.Compiled);

        private readonly Regex _additionOrSubstractionRegex = new Regex(@"((?:\(\-?\d+(?:\,\d+)?\)|(?:^[+|-])?\d+(?:\,\d+)?)(?:[E|e][+|-]?\d+)?)([+|-])((?:\(\-?\d+(?:\,\d+)?\)|\d+(?:\,\d+)?)(?:[E|e][+|-]?\d+)?)", RegexOptions.Compiled);

        private readonly Regex _valueRegex = new Regex(@"\-?\d+(?:\,\d+)?(?:[E|e][+|-]?\d+)?", RegexOptions.Compiled);

        private readonly Regex _infRegex = new Regex(@"\(?[+|-]?∞\)?", RegexOptions.Compiled);

        private readonly Regex _resultRegex = new Regex(@"^\(?(\-?\d+(?:\,\d+)?(?:[E|e][+|-]?\d+)?)\)?$", RegexOptions.Compiled);

        private readonly Regex _whitespaceRegex = new Regex(@"\s", RegexOptions.Compiled);

        private readonly Regex _coefficientRegex = new Regex(@"(?<value>\d)\(", RegexOptions.Compiled);

        private readonly Regex _minusRegex = new Regex(@"\-\(", RegexOptions.Compiled);

        #endregion

        /// <summary>
        /// Сalculates expression
        /// </summary>
        /// <param name="text">Expression for calculation</param>
        /// <returns>Result of calculation</returns>
        public double Calculate(string text)
        {
            string expression = text;
            if (expression.Count(s => s == '(') != expression.Count(s => s == ')'))
                throw new ArgumentException("\nWrong count of brackets!");

            expression = _whitespaceRegex.Replace(expression, "");
            expression = _coefficientRegex.Replace(expression, @"${value}*(");

            return Operations(expression);
        }

        private double Operations(string expression)
        {
            Match match;
            double result;
            expression = _minusRegex.Replace(expression, @"-1*(");
            while ((match = _bracketsRegex.Match(expression)).Success)
            {
                expression = expression.Remove(match.Index, match.Length);
                expression = (result = Operations(match.Groups[1].Value)) < 0 ? expression.Insert(match.Index, '(' + result.ToString("F2") + ')')
                    : expression.Insert(match.Index, result.ToString("F2"));
            }
            while ((match = _exponentiationRegex.Match(expression)).Success)
            {
                Calculate(match, Operation.Exponentiation, ref expression);
            }
            while ((match = _multiplicationOrDivisionRegex.Match(expression)).Success)
            {
                if (match.Groups[2].Value == "*")
                    Calculate(match, Operation.Multiplication, ref expression);
                else
                    Calculate(match, Operation.Division, ref expression);
            }

            while ((match = _additionOrSubstractionRegex.Match(expression)).Success)
            {
                if (match.Groups[2].Value == "+")
                    Calculate(match, Operation.Addition, ref expression);
                else
                    Calculate(match, Operation.Subtraction, ref expression);
            }

            if (_infRegex.IsMatch(expression))
            {
                throw new ArithmeticException("The possible division by 0 or the result of the calculation is infinity!");
            }
            if (!double.TryParse((match = _resultRegex.Match(expression)).Groups[1].Value, out result))
                throw new ArgumentException("\nWrong expression!");

            return result;
        }

        private void Calculate(Match match, Operation opreation, ref string expression)
        {
            double numLeft = double.Parse(_valueRegex.Match(match.Groups[1].Value).Value);
            double numRight = double.Parse(_valueRegex.Match(match.Groups[3].Value).Value);
            double result;

            switch (opreation)
            {
                case Operation.Exponentiation:
                    result = Math.Pow(numLeft, numRight);
                    break;
                case Operation.Multiplication:
                    result = numLeft * numRight;
                    break;
                case Operation.Division:
                    result = numLeft / numRight;
                    break;
                case Operation.Addition:
                    result = numLeft + numRight;
                    break;
                case Operation.Subtraction:
                    result = numLeft - numRight;
                    break;
                default:
                    throw new ArgumentException("\nInvalid operation!");
            }

            expression = expression.Remove(match.Index, match.Length);
            expression = result < 0 ? expression.Insert(match.Index, "(" + result + ")")
                : expression.Insert(match.Index, result.ToString());
        }
    }
}