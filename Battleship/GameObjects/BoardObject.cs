using Battleship.Enums;

namespace Battleship.GameObjects
{
    public class BoardObject : GameObject
    {
        public BoardObject()
        {
            Coords = new BoardStatuses[Grid, Grid];
        }

        public BoardStatuses[,] Coords { get; set; }

        /// <summary>
        /// Assesses if a space on the board is empty or occupied, assigns enum value: GUESSED or GUESSED_AND_HIT.
        /// </summary>
        /// <param name="x">The x coordinate on the board</param>
        /// <param name="y">The y coordinate on the board</param>
        /// <param name="flipped">Returns true or false, depending on if the coordinate value was flipped or not</param>
        public void FlipValue(int x, int y, out bool flipped)
        {
            switch (Coords[x, y])
            {
                case BoardStatuses.EMPTY:
                    Coords[x, y] = BoardStatuses.GUESSED;
                    flipped = true;
                    break;
                case BoardStatuses.OCCUPIED:
                    Coords[x, y] = BoardStatuses.GUESSED_AND_HIT;
                    flipped = true;
                    break;
                default:
                    flipped = false;
                    break;

            }
        }
    }
}
