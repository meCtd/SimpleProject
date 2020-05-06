using System;
using NUnit.Framework;


namespace SumOfNumbers.Tests
{
    [TestFixture]
    public class SumOfNumbersUnitTester
    {
        [Category("Correct string output")]
        [TestCase("000000000000000012e0000000000000000003", "1.2e+4")]
        [TestCase("12,23E-00000012", "1.223e-11")]
        [TestCase("1e0", "1.0e0")]
        [TestCase("99", "9.9e+1")]
        [TestCase("0", "0.0e0")]
        [TestCase("0,01", "1.0e-2")]
        public void TestToString(string inputString, string correctForm)
        {
            Assert.AreEqual(NewDouble.Input(inputString).ToString(), correctForm);

        }

        [Category("GetSum")]
        [TestCase("3e1", "2e2", "2.3e+2")]
        [TestCase("1000e10000", "23e-2", "1.0e+10003")]
        [TestCase("0,0000000000000001e1000", "00000000023,23e-1", "1.0e+984")]
        [TestCase("0", "0", "0.0e0")]
        [TestCase("2222e-1", "2222e1", "2.24422e+4")]
        [TestCase("000009,000000000000000000000000000000000000", "000000,123123123123123123123213000", "9.123123123123123123123213e0")]
        public void TestSum(string left, string right, string result)
        {
            Assert.AreEqual((NewDouble.Input(left) + NewDouble.Input(right)).ToString(), result);
        }

        [Category("Wrong input")]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("2,,,e-2")]
        [TestCase("2ee2")]
        [TestCase("2e+++2")]
        [TestCase("12.3r+2")]
        [TestCase("123,456e-e2")]
        [TestCase("999,222,2e1")]
        [TestCase("e2")]
        [TestCase("000009,0000000000000000000000000000000000000000000000000000000000000")]

        public void WrongInput(string input)
        {

            Assert.Throws<FormatException>(() => NewDouble.Input(input));
        }
        [Category("Check")]
        [TestCase("0","999","0.0e0")]
        [TestCase("12e9", "-1", "120000000000.0e-1")]
        [TestCase("0.1e-2", "2", "0.000010e+2")]
        [TestCase("999999999999999e10", "-20", "999999999999999000000000000000000000000000000.0e-20")]
        [TestCase("1000e2", "4", "10.0e+4")]

        public void CheckExponentChange(string input, string newExponent, string result)
        {
            NewDouble temp = NewDouble.Input(input);
            temp.SetExponent(int.Parse(newExponent));
            Assert.AreEqual(temp.ToString(),result);
        }
    }
}
