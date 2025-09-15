using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
