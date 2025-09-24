using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Battleship.Enums;

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

        /// <summary>
        /// Updates the cursor's position by 1 in a given direction, but prevents the cursor from going out of bounds.
        /// </summary>
        /// <param name="direction"></param>
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

        /// <summary>
        /// Pushes the cursor inbounds by a given length. <br/>
        /// <br/>
        /// Since ships are drawn from top to bottom or left to right, it will only check below and on the right side of the cursor's position.
        /// </summary>
        /// <param name="axis">The axis to assess</param>
        /// <param name="length">The hypothetical length of the cursor to assess</param>
        /// <exception cref="InvalidDataException"></exception>
        public void PushCursorInBounds(Rotations axis, int length)
        {
            switch (axis)
            {
                case Rotations.HORIZONTAL:
                    if (Position[0] + length > Grid)
                    {
                        Position[0] -= (Position[0] + length - Grid);
                    }
                    break;
                case Rotations.VERTICAL:
                    if (Position[1] + length > Grid)
                    {
                        Position[1] -= (Position[1] + length - Grid);
                    }
                    break;
                default:
                    throw new InvalidDataException($"Invalid data at PushCursorInBounds(): \"{axis}\" is not a valid value, expected \"Rotations.HORIZONTAL\" or \"Rotations.VERTICAL\"");
            }
        }
    }
}
