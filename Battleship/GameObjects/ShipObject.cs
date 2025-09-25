using Battleship.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.GameObjects
{
    public class ShipObject : GameObject
    {
        public ShipObject(int size, string name, char symbol) 
        {
            Size = size;
            Health = size;

            Name = name;
            Symbol = symbol;

            Coords = new BoardStatuses[Grid, Grid];
        }

        public bool IsPlaced = false;

        /// <summary>
        /// The amount of hits that the ship can take. intial value is identical to the ship's size.
        /// </summary>
        public int Health { get; set; }
        public int Size { get; }

        public string Name { get; private set; }

        /// <summary>
        /// The symbol that gets drawn to the console.
        /// </summary>
        public char Symbol { get; }

        /// <summary>
        /// [0] = x <br/>
        /// [1] = y <br/>
        /// </summary>
        public BoardStatuses[,] Coords { get; set; }
    }
}
