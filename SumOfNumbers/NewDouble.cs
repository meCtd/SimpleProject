using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SumOfNumbers
{
    internal class NewDouble
    {

        /// <summary>
        /// The exponent of a number
        /// </summary>
        private int _exponent;
        /// <summary>
        /// All numbers before exponent (-1 in array mean that it is a ','or '.')
        /// </summary>
        private readonly List<int> _nums;

        public static NewDouble Empty => new NewDouble(0, new[] { 0, -1, 0 });

        private NewDouble(int exponent, IEnumerable<int> nums)
        {
            _exponent = exponent;
            _nums = nums.ToList();
        }

        public static NewDouble Input(string text)
        {
            // Type of the number ==> one or more natural digits -> '.' or ',' -> from one to 40 digits -> 'E' or 'e' -> one or more digit
            Match match = Regex.Match(text, @"^(\d+(?:[\.|,]?\d{1,39})?)(?:[E|e]([+|-]?\d+)?)?$", RegexOptions.Compiled);

            List<int> nums;
            int exp;

            if (match.Success)
            {
                // take an numbers before 'E' or 'e' and conver to int list, where separator is '.' or ','
                nums = match.Groups[1].Value.Select(s => (int)char.GetNumericValue(s)).ToList();

                //If text doesnt contain separator (4e22, or 12e-1)add an separator and zero ( 4e12 ->4,0e12)
                if (!nums.Contains(-1))
                {
                    nums.AddRange(new[] { -1, 0 });
                }
                if (!text.Contains('e') && !text.Contains('E'))
                    exp = 0;
                // Take an numbers after 'E' or 'e'
                else
                    exp = int.Parse(match.Groups[2].Value);
            }
            else
            {
                throw new FormatException("Invalid format");
            }

            NewDouble result = new NewDouble(exp, nums);
            result.SetNormalForm();
            result.Trim();
            return result;

        }

        public static NewDouble operator +(NewDouble elem1, NewDouble elem2)
        {
            int resultExponent = (elem1._exponent + elem2._exponent) / 2;

            // Set the same exponential in both numbers 
            if (elem1._exponent != elem2._exponent)
            {
                elem1.SetExponent(resultExponent);
                elem2.SetExponent(resultExponent);
            }

            //Synchronization of elements in nums
            int index1 = elem1._nums.IndexOf(-1);
            int index2 = elem2._nums.IndexOf(-1);

            while (index1 != index2)
            {
                if (index1 > index2)
                {
                    elem2._nums.Insert(0, 0);
                    index2++;
                }
                else
                {
                    elem1._nums.Insert(0, 0);
                    index1++;
                }
            }

            while (elem1._nums.Count != elem2._nums.Count)
            {
                if (elem1._nums.Count > elem2._nums.Count)
                    elem2._nums.Insert(elem2._nums.Count, 0);
                else
                    elem1._nums.Insert(elem1._nums.Count, 0);
            }

            // Addition
            int[] resultNums = new int[elem1._nums.Count + 1];

            for (int i = elem1._nums.Count - 1; i >= 0; i--)
            {
                if (elem1._nums[i] == -1)
                {
                    if (resultNums[i + 1] != 0)
                        resultNums[i]++;

                    resultNums[i + 1] = -1;
                    continue;
                }

                resultNums[i + 1] += elem1._nums[i] + elem2._nums[i];

                if (resultNums[i + 1] >= 10)
                {
                    resultNums[i + 1] -= 10;
                    resultNums[i]++;
                }

            }

            NewDouble result = new NewDouble(resultExponent, resultNums);
            result.SetNormalForm();
            return result;
        }

        private bool IsEmpty()
        {
            this.Trim();
            if (this._nums.Count != NewDouble.Empty._nums.Count)
            {
                return false;
            }

            for (int i = 0; i < 3; i++)
            {
                if (_nums[i] != NewDouble.Empty._nums[i])
                    return false;
            }

            return true;
        }



        /// <summary>
        /// Change an exponent in number
        /// </summary>
        /// <param name="exp">New exponent</param>
        /// 
        public void SetExponent(int exp)
        {
            if (IsEmpty())
            {
                _exponent = 0;
                return;
            }
            else
            {
                if (_exponent < exp)
                {
                    for (int i = _nums.IndexOf(-1); exp != _exponent; i--, _exponent++)
                    {
                        if (i == 0)
                        {
                            _nums.Insert(0, 0);
                            i++;
                        }

                        _nums[i] = _nums[i - 1];
                        _nums[i - 1] = -1;

                    }
                    if (_nums[0] == -1)
                        _nums.Insert(0, 0);
                }
                else
                {
                    for (int i = _nums.IndexOf(-1); exp != _exponent; i++, _exponent--)
                    {
                        if (i == _nums.Count - 1)
                            _nums.Insert(_nums.Count, 0);

                        _nums[i] = _nums[i + 1];
                        _nums[i + 1] = -1;

                    }
                    if (_nums[_nums.Count - 1] == -1)
                        _nums.Insert(_nums.Count, 0);
                }
            }
        }

        /// <summary>
        /// Sets an normal exponential number form (was 19,0E+1 -->1,9e2), use it after addition
        /// </summary>
        public void SetNormalForm()
        {
            int separator = _nums.IndexOf(-1);
            int firstNum = _nums.FindIndex(s => (s != -1) && (s != 0));

            // If the separator is not before the first non-zero number
            if ((firstNum + 1) != separator && firstNum != -1)
            {
                _nums.RemoveAt(separator);

                //if separator is before the first non-zero number (0,0000001 or 0,001), otherwise - 123.345 or 11111,00001
                if (separator < firstNum)
                {
                    _nums.Insert(firstNum, -1);
                    _exponent -= firstNum - separator;
                }
                else
                {
                    _nums.Insert(firstNum + 1, -1);
                    _exponent += (separator - firstNum - 1);
                }

            }

            if (_nums.Count > 41)
                _nums.RemoveRange(41, _nums.Count - 41);

            Trim();
        }

        /// <summary>
        /// Removes extra zeros at the edges
        /// </summary>
        private void Trim()
        {
            //Remove zero numbers from start
            int firstDigit = _nums.FindIndex(s => (s != -1) && (s != 0));

            if (firstDigit != -1)
                _nums.RemoveRange(0, firstDigit);
            else
                _nums.RemoveRange(0, _nums.IndexOf(-1) - 1);


            //Remove zero numbers from end
            int lastDigit = _nums.FindLastIndex(s => (s != 0));
            if (lastDigit != -1)
            {
                // remove all zeros from the end
                _nums.RemoveRange(lastDigit + 1, _nums.Count - lastDigit - 1);

                // if no non-zero numbers after separator - add zero after separator
                if (_nums.IndexOf(-1) == _nums.Count - 1)
                    _nums.Add(0);
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (var el in _nums)
                if (el == -1)
                    result.Append('.');
                else
                    result.Append(el);

            result.Append($"e{_exponent:+#;-#;0}");
            return result.ToString();
        }
    }
}