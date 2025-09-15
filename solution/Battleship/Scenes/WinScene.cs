using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Battleship.GameObjects;

namespace Battleship.Scenes
{
    internal class WinScene : Scene
    {
        private PlayerObject? WinningPlayer = Players.SingleOrDefault(player => player.Points == 5);

        private enum Options
        {
            RESTART,
            EXIT
        };

        private Options Selected = Options.RESTART;

        public override void Update()
        {
            switch (KeyboardHandler.RequestInput())
            {
                case ConsoleKey.RightArrow:
                    if (Selected == Options.EXIT) Selected--; else Selected++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (Selected == Options.RESTART) Selected++; else Selected--;
                    break;
                default:
                    switch (Selected)
                    {
                        case Options.RESTART:
                            RequestedScene = new SetupScene();
                            return;
                        case Options.EXIT:
                            Terminate();
                            return;
                    }
                    break;
            }
        }

        public override void Draw()
        {
            for (int y = 1; y < Height; y++)
            {
                if (y > 1) Console.Write("|"); else Console.Write(" ");

                if (y == Height / 2 - 5)
                {
                    Console.Write(WhitespaceAround($"{((WinningPlayer != null) ? $"{WinningPlayer.Name} HAS WON!" : "Oops something went wrong!")}", Width - 1));
                }
                else if (y == Height / 2)
                {
                    switch (Selected)
                    {
                        case Options.RESTART:
                            Console.Write(WhitespaceAround("|RESTART|           EXIT ", Width - 1));
                            break;
                        case Options.EXIT:
                            Console.Write(WhitespaceAround(" RESTART           |EXIT|", Width - 1));
                            break;
                    }
                }
                else
                {
                    for (int x = 1; x < Width; x++)
                    {
                        if (y == 1)
                        {
                            Console.Write("_");
                        }
                        else if (y == Height - 1)
                        {
                            Console.Write("_");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                }

                if (y > 1) Console.Write("|");
                else Console.Write(" ");

                Console.WriteLine();
            }
        }
    }
}
