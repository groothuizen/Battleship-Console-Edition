namespace Battleship.Handlers
{
    public class KeyboardHandler : Handler
    {
        public ConsoleKey RequestInput()
        {
            return Console.ReadKey(true).Key;
        }
    }
}
