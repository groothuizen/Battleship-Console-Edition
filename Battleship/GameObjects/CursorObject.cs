using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.GameObjects
{
    public class CursorObject : GameObject
    {
        public string Symbol = String.Empty;

        /// <summary>
        /// [0] = x,
        /// [1] = y
        /// </summary>
        public int[] Position { get; } = new int[2] { 1, 1 };

        public void Move(string direction)
        {
            switch (direction)
            {
                case "up":
                    if (Position[1] != 0) Position[1]--;
                    break;
                case "right":
                    if (Position[0] < Grid - 1) Position[0]++;
                    break;
                case "down":
                    if (Position[1] < Grid - 1) Position[1]++;
                    break;
                case "left":
                    if (Position[0] != 0) Position[0]--;
                    break;
            }
        }

        public void PushCursorInBounds(int axis, int length)
        {
            switch (axis)
            {
                case 0:
                    if (Position[0] + length > Grid)
                    {
                        Position[0] -= (Position[0] + length - Grid);
                    }
                    break;
                case 1:
                    if (Position[1] + length > Grid)
                    {
                        Position[1] -= (Position[1] + length - Grid);
                    }
                    break;
                default:
                    throw new InvalidDataException($"Invalid data at PushCursorInBounds(): \"{axis}\" is not a valid value, expected \"0\" for x axis or \"1\" for y axis");
            }
        }
    }
}
