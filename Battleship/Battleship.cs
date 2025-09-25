using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleLog;

using Battleship.Handlers;
using Battleship.Scenes;

namespace Battleship
{
    class Battleship
    {
        public static void Main(string[] args)
        {
            LogService.StartSerilog();

            SceneHandler sceneHandler = new();

            sceneHandler.Start(new TitleScene());

            bool isAlive = true;

            do
            {
                do
                {
                    sceneHandler.Update();
                } while (Console.KeyAvailable); // Prevents pauses between inputs
            } while (isAlive);

            Console.WriteLine("\nTerminating program...");
        }
        
    }
}