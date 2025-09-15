using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.GameObjects
{
    public class BoardObject : GameObject
    {
        public BoardObject()
        {
            Values = new int[Grid, Grid];
        }

        public int[,] Values { get; set; }

        /// <summary>
        /// Checks if a space on the board is empty or occupied, assigns a value and then returns true, otherwise returns false.
        /// </summary>
        /// <param name="x">x coordinate on the board</param>
        /// <param name="y">y coordinate on the board</param>
        /// <returns></returns>
        public void FlipValue(int x, int y, out bool flipped)
        {
            switch (Values[x, y])
            {
                case 0:
                    Values[x, y] = 2;
                    flipped = true;
                    break;
                case 1:
                    Values[x, y] = 3;
                    flipped = true;
                    break;
                default:
                    flipped = false;
                    break;

            }
        }
    }
}
