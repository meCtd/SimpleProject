using System;

namespace BattleShip
{
    internal static class Program
    {
        static Program()
        {
            Console.Title = "Sea battle";
        }

        static void Main()
        {
            Console.CursorVisible = false;
            try
            {
                GameController gameController = new GameController(Environment.UserName,new ConsoleDrawer());
                gameController.MenuController();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.CursorVisible = true;
                Console.ReadKey();
            }
        }
    }
}
