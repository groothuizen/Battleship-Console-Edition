namespace Battleship.Scenes
{
    internal class TitleScene : Scene
    {
        private enum Options
        {
            START,
            EXIT
        };

        private Options Selected = Options.START;

        public override void Update()
        {
            switch (KeyboardHandler.RequestInput())
            {
                case ConsoleKey.RightArrow:
                    if (Selected == Options.EXIT) Selected--; else Selected++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (Selected == Options.START) Selected++; else Selected--;
                    break;
                default:
                    switch (Selected)
                    {
                        case Options.START:
                            RequestedScene = new SettingsScene();
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

                if (y == Height / 2)
                {
                    switch (Selected)
                    {
                        case Options.START:
                            Console.Write(WhitespaceAround("|START|           EXIT ", Width - 1));
                            break;
                        case Options.EXIT:
                            Console.Write(WhitespaceAround(" START           |EXIT|", Width - 1));
                            break;
                    }
                }

                if (y > 1) Console.Write("|"); 
                else Console.Write(" ");

                Console.WriteLine();
            }
        }
    }
}
