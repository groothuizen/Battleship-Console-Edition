using Battleship.GameObjects;

namespace Battleship.Scenes
{
    internal class ResultScene : Scene
    {
        public ResultScene() 
        {
            if (WinningPlayer != null)
            {
                Serilog.Log.Information
                (
                    "#RESULT {winningPlayerName} has won! Board status end result: {boardStatus}",
                    WinningPlayer.Name, WinningPlayer.Board.Coords
                );

                Serilog.Log.Information
                (
                    "#RESULT {losingPlayerName} has lost with {points} points! Board status end result: {boardStatus}",
                    LosingPlayer.Name, LosingPlayer.Points, LosingPlayer.Board.Coords
                );
            }
            else
            {
                Serilog.Log.Error("#RESULT no winner could be found...");
            }
        }

        private PlayerObject? WinningPlayer = Players.SingleOrDefault(player => player.HasWon());
        private PlayerObject? LosingPlayer = Players.SingleOrDefault(player => !player.HasWon())!;

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
