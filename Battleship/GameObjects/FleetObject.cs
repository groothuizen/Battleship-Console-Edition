using Battleship.Enums;

namespace Battleship.GameObjects
{
    /// <summary>
    /// Includes a collection of Ship objects and some useful methods.
    /// </summary>
    public class FleetObject : GameObject
    {
        public FleetObject() 
        {
            Ships = new ShipObject[5]
            {
                new(size: 2, name: "Destroyer", symbol: '1'),
                new(size: 3, name: "Submarine", symbol: '2'),
                new(size: 3, name: "Cruiser", symbol: '3'),
                new(size: 3, name: "Battleship", symbol: '4'),
                new(size: 4, name: "Aircraft Carrier", symbol: '5')
            };
        }

        public ShipObject[] Ships { get; private set; }

        /// <summary>
        /// A pointer for the Ship object List.
        /// </summary>
        public int ShipPointer { get; private set; } = 0;

        /// <summary>
        /// The current rotation state from the enum: "Rotations". <br/>
        /// <br/>
        /// Default: "Rotations.HORIZONTAL".
        /// </summary>
        public Rotations Rotation { get; set; } = Rotations.HORIZONTAL;

        /// <summary>
        /// Rotates the ship by changing the value of the Rotation property.
        /// </summary>
        public Rotations Rotate() => (Rotation == Rotations.HORIZONTAL) ? Rotation = Rotations.VERTICAL : Rotation = Rotations.HORIZONTAL;

        public ShipObject GetCurrentShip() => Ships[ShipPointer];

        /// <summary>
        /// Assess if a ship is currently occupying a given coordinate.
        /// </summary>
        /// <param name="x">The x coordinate on the board</param>
        /// <param name="y">The y coordinate on the board</param>
        /// <returns>ShipObject</returns>
        public ShipObject? FindShipFromPosition(int x, int y)
        {
            return Ships.SingleOrDefault(ship => ship.Coords[x, y] == BoardStatuses.OCCUPIED);
        }

        /// <summary>
        /// Increments the value of the ship pointer. <br/>
        /// <br/>
        /// Resets to the index value of the first unplaced ship, when the value exceeds the length of the ships List.
        /// </summary>
        public void ShiftShipPointer()
        {
            for (int i = 1; i < Ships.Length + 1; i++)
            {
                if (ShipPointer + i < Ships.Length)
                {
                    if (!Ships[ShipPointer + i].IsPlaced)
                    {
                        ShipPointer += i;
                        return;
                    }
                }
                else
                {
                    if (!Ships[ShipPointer - Ships.Length + i].IsPlaced)
                    {
                        ShipPointer = ShipPointer - Ships.Length + i;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a ship is inbounds, according to size and rotation.
        /// </summary>
        /// <param name="x">The x coordinate on the board</param>
        /// <param name="y">The y coordinate on the board</param>
        /// <returns>true or false</returns>
        /// <exception cref="InvalidDataException"></exception>
        public bool ShipInBounds(int x, int y)
        {
            if (x >= 0 && x < Grid && y >= 0 && y < Grid)
            {
                switch (Rotation)
                {
                    case Rotations.HORIZONTAL:
                        if (x + Ships[ShipPointer].Size > Grid) return false;
                        else return true;
                    case Rotations.VERTICAL:
                        if (y + Ships[ShipPointer].Size > Grid) return false;
                        else return true;
                    default:
                        throw new InvalidDataException($"Invalid data at ShipInBounds(): \"{Rotation}\" is not a valid value, expected \"Rotations.HORIZONTAL\" or \"Rotations.VERTICAL\"");
                }
            }
            else
            {
                return false;
            }
        }
    }
}
