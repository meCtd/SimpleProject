
using System;
using NUnit.Framework;

namespace Calculator.Tests
{
    public class CalculatorUnitTests
    {
        [Category("CheckForResult")]
        [TestCase("2+2*2", 6)]
        [TestCase("(2+2)*2", 8)]
        [TestCase("14+7*(2^3)-(1*2-3+4^(2-4*7*(3*(1,123+2,33))))", 71)]
        [TestCase("1-(-(-(-0,5)))", 1.5)]
        [TestCase("2^3/4+2/(2+2)+3^1/(6^(14-7*(1+1))+5)", 3)]
        [TestCase("-(-(-(-(1+(-2)^(-(12+(-11)))))))", 0.5)]
        public void CheckForResult(string input, double result)
        {
            Assert.AreEqual(new StringCalc().Calculate(input), result);
        }

        [Category("CheckForBrackets")]
        [TestCase("2+(2+2))")]
        [TestCase("2+(2+2))")]
        [TestCase("7-)3")]
        [TestCase("-(((-((2))))")]
        [TestCase("(2+2")]
        [TestCase("123+(1234)))")]
        public void CheckForBrackets(string input)
        {
            Assert.Throws<ArgumentException>(() => { new StringCalc().Calculate(input); });
        }

        [Category("WrongExpression")]
        [TestCase("2++2")]
        [TestCase(")2+2(*2")]
        [TestCase("2e12+-^123e123")]
        [TestCase("1,2eE12+3")]
        [TestCase("0,00000+123,12312,2e12+(2+(023))")]
        public void CheckForExpression(string expression)
        {
            Assert.Throws<ArgumentException>(() => new StringCalc().Calculate(expression));
        }

        [Category("Possible Infinity")]
        [TestCase("2/0")]
        [TestCase("2+2*2+(12.23/0)")]
        [TestCase("(2*213)/0+123")]
        public void CheckForInf(string expression)
        {
            Assert.Throws<ArithmeticException>(() => new StringCalc().Calculate(expression));

        }
    }
}
