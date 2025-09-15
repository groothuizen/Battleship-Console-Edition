using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Battleship.GameObjects;
using Battleship.Handlers;
using Battleship.Interfaces;

namespace Battleship.Scenes
{
    /// <summary>
    /// The Scene class oversees the logic, objects and drawing in the game.
    /// </summary>
    public abstract class Scene : IScene
    {
        public Scene() 
        {
            Grid = ConfigHandler.GetInt("Grid");
            GridGapX = Grid;
            GridGapY = Grid / 2;
            Width = (Grid + 1) * GridGapX;
            Height = (Grid + 1) * GridGapY;

            Console.SetWindowSize(Width + 60, Height);
        }

        // ===== Static Game Objects =====
        // Instantiated inside Scene to retain data between scenes.
        public static PlayerObject[] Players { get; set; } = 
        [
            new PlayerObject("Player 1"),
            new PlayerObject("Player 2")
        ];

        public static CursorObject Cursor { get; set; } = new();

        public KeyboardHandler KeyboardHandler { get; } = new();
        public ConfigHandler ConfigHandler { get; } = new();

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Grid { get; private set; }
        public int GridGapX { get; private set; }
        public int GridGapY { get; private set; }

        public bool IsAlive { get; private set; } = true;
        public void Terminate() => IsAlive = false;

        public Scene? RequestedScene { get; set; }

        public abstract void Update();
        public abstract void Draw();

        /// <summary>
        /// Writes whitespace around the text and aligns the text in the middle of the predetermined width value.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string? WhitespaceAround(string text, int width)
        {
            if (text != null)
            {
                int remainingWidth = width - text.Length;
                string[] whitespaceArray = new string[width];

                for (int i = 0; i < remainingWidth / 2; i++)
                {
                    whitespaceArray[i] = " ";
                }
                string whitespace = string.Join("", whitespaceArray);

                string joinedString = whitespace + text + whitespace;

                if (joinedString.Length < width) joinedString += " ";

                return joinedString;
            } else
            {
                return string.Empty;
            }
        }
    }
}
