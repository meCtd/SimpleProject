using System;
using System.Diagnostics;

namespace BattleShip
{
    [Serializable]
    internal class ConsoleDrawer : IDraw
    {
        public void DrawPicture(SeaBattleGame currentGame)
        {
            Console.SetCursorPosition(0, 0);
            DrawBoard(currentGame);

        }

        /// <summary>
        /// Draw game boards
        /// </summary>
        /// <param name="sea">Current game</param>
        private void DrawBoard(SeaBattleGame sea)
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    BoardDrawer(sea.HumanPlayer.OpponentsBoard, i, j);
                }

                for (int j = 0; j < 12; j++)
                {
                    BoardDrawer(sea.HumanPlayer.MyBoard, i, j);
                }

                if (sea.Hack)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        BoardDrawer(sea.ComputerPlayer.MyBoard, i, j);
                    }
                }
                Console.WriteLine();
            }

            DrawAim(sea.HumanPlayer.CurrentPosition);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(0, 12);
        }

        private void BoardDrawer(Board board, int i, int j)
        {

            switch (board.BoardElements[i, j])
            {
                case BoardElementType.Border:
                    Console.ForegroundColor = ConsoleColor.Blue;

                    if (i == 0 && j == 0)
                        Console.Write(" ╔");
                    else if (i == 11 && j == 0)
                        Console.Write(" ╚");
                    else if (i == 0 && j == 11)
                        Console.Write("╗ ");
                    else if (i == 11 && j == 11)
                        Console.Write("╝ ");
                    else if ((i == 0 || i == 11) && (j != 0 && j != 11))
                        Console.Write("══");
                    else if (j == 0)
                        Console.Write(" ║");
                    else
                        Console.Write("║ ");

                    break;
                case BoardElementType.Empty:
                    Console.Write("::");
                    break;
                case BoardElementType.KilledShip:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("██");
                    break;
                case BoardElementType.NotChecked:
                    Console.Write("  ");
                    break;
                case BoardElementType.WoundedShip:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("██");
                    break;
                case BoardElementType.Ship:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("██");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Draw an aim around position
        /// </summary>
        /// <param name="choosenPosition">Choosen position</param>
        private void DrawAim(Position choosenPosition)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(choosenPosition.Y * 2 - 2, choosenPosition.X - 1);
            Console.Write(" ┌");
            Console.SetCursorPosition(choosenPosition.Y * 2 - 2, choosenPosition.X + 1);
            Console.Write(" └");
            Console.SetCursorPosition(choosenPosition.Y * 2 + 2, choosenPosition.X - 1);
            Console.Write("┐ ");
            Console.SetCursorPosition(choosenPosition.Y * 2 + 2, choosenPosition.X + 1);
            Console.Write("┘ ");
        }

    }
}
