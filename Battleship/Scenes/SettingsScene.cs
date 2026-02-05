using Battleship.Enums;

namespace Battleship.Scenes
{
    internal class SettingsScene : Scene
    {
        private PlayerTurnStates CurrentPlayer = PlayerTurnStates.PLAYER1;

        public override void Update()
        {
            Console.Write(" > ");
            string? name = Console.ReadLine();

            if (name != null && name != String.Empty)
            {
                Players[(int)CurrentPlayer].Name = name;
            }
            else
            {
                Players[(int)CurrentPlayer].Name = (CurrentPlayer == PlayerTurnStates.PLAYER1) ? "Player 1" : "Player 2";
            }

            if (CurrentPlayer == PlayerTurnStates.PLAYER1)
            {
                CurrentPlayer = PlayerTurnStates.PLAYER2;
            }
            else
            {
                RequestedScene = new SetupScene();
            }
        }

        public override void Draw()
        {
            var currentPlayer = Players[(int)CurrentPlayer];
            for (int y = 1; y < Height; y++)
            {
                if (y > 1) Console.Write("|"); else Console.Write(" ");

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
                        if (y != Height / 2)
                        {
                            Console.Write(" ");
                        }
                    }
                }

                if (y == Height / 2) Console.Write(WhitespaceAround($"{((CurrentPlayer == PlayerTurnStates.PLAYER1) ? "Player 1" : "Player 2")}, please enter your name and press enter...", Width - 1));

                if (y > 1) Console.Write("|");
                else Console.Write(" ");

                Console.WriteLine();
            }
        }
    }
}
