using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BattleShip
{
    [Serializable]
    internal class GameController
    {
        [NonSerialized]
        public readonly string ProfileName;

        private SeaBattleGame _game;

        private IDraw _gameDrawer;

        public GameController(string profileName,IDraw gameDrawer)
        {
            ProfileName = profileName;
            _gameDrawer = gameDrawer;
        }

        private void MenuItems()
        {
            Console.Clear();

            Console.WriteLine($"Hello {ProfileName}");
            Console.WriteLine("\t1-> New game");
            Console.WriteLine("\t2-> Continue current game");
            Console.WriteLine("\t3-> Save current game");
            Console.WriteLine("\t4-> Open saved game");
            Console.WriteLine("\t5-> About game");
            Console.WriteLine("\tEsc-> Exit\n");
        }

        public void NewGame()
        {
            _game = new SeaBattleGame();
        }
        public void MenuController()
        {
            MenuItems();
            while (true)
            {
                ConsoleKey pressedKey = Console.ReadKey(true).Key;

                for (int i = Console.CursorTop; i > 7; i--)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write(new string(' ', Console.BufferWidth));
                }
                switch (pressedKey)
                {
                    case ConsoleKey.D1:
                        NewGame();
                        StartGame();
                        MenuItems();
                        break;

                    case ConsoleKey.D2:

                        if (_game != null)

                            StartGame();
                        else
                        {
                            Console.Write("Game is not started!");
                        }
                        break;

                    case ConsoleKey.D3:

                        if (_game != null)
                        {

                            Console.WriteLine("Enter the save path: ");
                            Console.WriteLine("File path " + SaveGame(Console.ReadLine()));

                        }
                        else
                            Console.Write("Game is not started!");

                        break;
                    case ConsoleKey.D4:
                        Console.Write("Enter the path of saved game ->");
                        OpenGame(Console.ReadLine());
                        break;

                    case ConsoleKey.D5:

                        Console.Write("Sea Battle by Ctd 2018");
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            }
        }
        private void StartGame()
        {
            Console.Clear();
            _gameDrawer.DrawPicture(_game);

            while (true)
            {
                if (_game.CurrentTurn)
                {
                    ConsoleKey pressedKey;
                    do
                    {
                        switch (pressedKey = Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.UpArrow:
                                if (_game.HumanPlayer.CurrentPosition.X != 1)
                                    _game.HumanPlayer.CurrentPosition.X--;
                                break;


                            case ConsoleKey.DownArrow:
                                if (_game.HumanPlayer.CurrentPosition.X != 10)
                                    _game.HumanPlayer.CurrentPosition.X++;
                                break;


                            case ConsoleKey.LeftArrow:
                                if (_game.HumanPlayer.CurrentPosition.Y != 1)
                                    _game.HumanPlayer.CurrentPosition.Y--;
                                break;


                            case ConsoleKey.RightArrow:
                                if (_game.HumanPlayer.CurrentPosition.Y != 10)
                                    _game.HumanPlayer.CurrentPosition.Y++;
                                break;

                            case ConsoleKey.F12:
                                _game.Hack = !_game.Hack;
                                Console.Clear();
                                break;

                            case ConsoleKey.Escape:
                                MenuItems();
                                return;
                        }

                        _gameDrawer.DrawPicture(_game);
                    } while (pressedKey != ConsoleKey.Spacebar);
                }
                _game.Turn();
                _gameDrawer.DrawPicture(_game);
                //Check for winner
                if (_game.HumanPlayer.MyBoard.Ships.Count(s => s.Health > 0) == 0)
                {
                    Console.WriteLine("You lose!");
                    //delay
                    Console.ReadKey(true);
                    _game = null;
                    return;
                }
                if (_game.ComputerPlayer.MyBoard.Ships.Count(s => s.Health > 0) == 0)
                {
                    Console.WriteLine("You win");
                    //delay
                    Console.ReadKey(true);
                    _game = null;
                    return;
                }

            }
        }

        /// <summary>
        /// Save the current game in Binary-format
        /// </summary>
        public string SaveGame(string path)
        {
            string savedPath = String.Empty;
            try
            {

                if (_game != null)
                {
                    BinaryFormatter saveGame = new BinaryFormatter();

                    using (FileStream saveFileStream =
                        new FileStream($"{path}\\{ProfileName}_SavedGame.svd", FileMode.Create))
                    {
                        saveGame.Serialize(saveFileStream, _game);
                        savedPath = saveFileStream.Name;
                    }
                }

                Console.WriteLine("Game was saved successful!");
                return savedPath;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Wrong path!");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("No access to this folder");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Folder is not found!");
            }
            return savedPath;
        }

        /// <summary>
        /// Open game from file
        /// </summary>
        /// <param name="path">Path of file</param>
        public void OpenGame(string path)
        {
            BinaryFormatter openGame = new BinaryFormatter();
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    _game = (SeaBattleGame)openGame.Deserialize(stream);
                }

                Console.WriteLine("Game was opened successful! Now you can continue your past game!");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error! file is not found");
            }
            catch (SerializationException)
            {
                Console.WriteLine("Error! This file is invalid!");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Empty string is not allowed!");
            }

        }
    }
}
