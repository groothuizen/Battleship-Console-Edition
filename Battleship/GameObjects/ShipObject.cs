using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.GameObjects
{
    public class ShipObject : GameObject
    {
        public ShipObject(int size, string name, string symbol) 
        {
            Size = size;
            Health = size;

            Name = name;
            Symbol = symbol;

            Positions = new int[Grid, Grid];
        }

        public bool IsPlaced = false;

        public int Health { get; set; }
        public int Size { get; }

        public string Name { get; private set; }
        public string Symbol { get; }

        /// <summary>
        /// [0] = x,
        /// [1] = y
        /// </summary>
        public int[,] Positions { get; set; }
    }
}
