using System;

namespace SumOfNumbers
{
    internal static class Program
    {
        static Program()
        {
            Console.Title = "Sum of numbers";
        }
        private static void Main(string[] args)
        {
            Console.WriteLine("Input numbers in exexpontntal form (1,3e2 or 0,000023E-23)");
            Console.WriteLine("If you want to stop addition press '='\n");


            NewDouble a = NewDouble.Empty;

            do
            {
                Console.Write("Input==>");               
                string text = Console.ReadLine();
                if (text == "=")
                    break;

                NewDouble b;
                try
                {
                    b = NewDouble.Input(text);
                }

                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                    b = NewDouble.Empty;
                }

                a = a + b;

            } while (true);

            Console.WriteLine($"Result==> {a}");
            Console.ReadKey();
        }
    }
}
