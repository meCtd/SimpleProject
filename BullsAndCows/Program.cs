using System;

namespace BullsAndCows
{
    internal static class Program
    {
        static Program()
        {
            Console.Title = "Bulls and Cows";
        }
        static void Main(string[] args)
        {
            bool again;
            Game newGame = new Game();
            do
            {
                try
                {
                    Console.Write("Input count of digits: ");
                    newGame.StartGame(Input());

                    do
                    {
                        Console.WriteLine($"Step #{newGame.StepCount}");
                        Console.WriteLine($"Your number is {newGame.PossibleResult}");

                        Console.Write("Bulls:");
                        int bullCount = Input();

                        Console.Write("Cows:");
                        int cowCount = Input();

                        newGame.NextTurn(bullCount, cowCount);
                        Console.WriteLine(new string('-',30));

                    } while (newGame.Result == string.Empty);

                Console.WriteLine($"Result is {newGame.Result}");
            }
                catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Play again? Press Y/N\n");
                again = char.ToUpperInvariant(Console.ReadKey(true).KeyChar) == 'Y';
            }

        }
            while (again);
        }

        private static int Input()
        {
            int value = -1;
            bool flag = false;
            do
            {
                flag = true;

                if (!int.TryParse(Console.ReadLine(), out value))
                {
                    flag = false;
                    throw new ArgumentException("Wrong Input!");
                }
                    
            } while (!flag);

            return value;
        }
    }
}
