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
    /// The Scene class oversees the logic, objects and drawing of the game.
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
        // Instantiated inside Scene to retain data between scenes and to avoid memory leaks.
        public static PlayerObject[] Players { get; set; } = 
        [
            new PlayerObject("Player 1"),
            new PlayerObject("Player 2")
        ];

        public static CursorObject Cursor { get; set; } = new();


        // ===== Handlers =====
        public KeyboardHandler KeyboardHandler { get; } = new();
        public ConfigHandler ConfigHandler { get; } = new();


        // ===== Global Scene Properties =====
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Grid { get; private set; }
        public int GridGapX { get; private set; }
        public int GridGapY { get; private set; }


        // ===== Scene Operation State =====
        public bool IsAlive { get; private set; } = true;
        public void Terminate() => IsAlive = false;

        /// <summary>
        /// Requests a given scene and the scene handler notices this change.
        /// </summary>
        public Scene? RequestedScene { get; set; }

        /// <summary>
        /// Contains all logic to update the scene with. <br/>
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Draws the scene in the console window. <br/>
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Adds whitespace around the text value, depending on the width parameter.
        /// </summary>
        /// <param name="width">The total width to fill</param>
        /// <returns>new string</returns>
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
