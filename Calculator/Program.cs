using System;
using System.Linq;

namespace Calculator
{
    internal static class Program
    {
        static Program()
        {
            Console.Title = "String Calulator";
        }
        private static void Main(string[] args)
        {
            char[] operations = { ',', '+', '-', '/', '*', '^', '(', ')', 'E', 'e' };
            do
            {
                try
                {
                    string expression = string.Empty;
                    char a;
                    while ((a = Console.ReadKey(true).KeyChar) != '=')
                    {
                        if (char.IsDigit(a) || operations.Contains(a))
                        {
                            expression += a;
                            Console.Write(a);
                        }

                        if (a == '\b' && expression.Length != 0)
                        {
                            expression = expression.Remove(expression.Length - 1);
                            Console.Write("\b \b");
                        }
                    }

                    Console.Write('=');
                    StringCalc calculator = new StringCalc();
                    
                    Console.WriteLine(calculator.Calculate(expression));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (OverflowException)
                {
                    Console.WriteLine("\nOverflow error in the calculation");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("Try again?[Esc to exit]");
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
