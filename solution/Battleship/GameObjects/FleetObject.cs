using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.GameObjects
{
    public class FleetObject : GameObject
    {
        public FleetObject() 
        {
            Ships = new ShipObject[5]
            {
                new(size: 2, "Destroyer", "1"),
                new(size: 3, "Submarine", "2"),
                new(size: 3, "Cruiser", "3"),
                new(size: 3, "Battleship", "4"),
                new(size: 4, "Aircraft Carrier", "5")
            };
        }

        public ShipObject[] Ships { get; private set; }
        public int ShipPointer { get; private set; } = 0;

        /// <summary>
        /// 0 for horizontal or 1 for vertical
        /// </summary>
        public int Rotation { get; set; } = 0;

        public void Rotate()
        {
            if (Rotation == 0)
            {
                Rotation = 1;
            }
            else
            {
                Rotation = 0;
            }
        }

        public ShipObject GetCurrentShip() => Ships[ShipPointer];

        public ShipObject? FindShipFromPosition(int x, int y)
        {
            return Ships.SingleOrDefault(ship => ship.Positions[x, y] == 1);
        }

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

        public bool ShipInBounds(int x, int y)
        {
            if (x >= 0 && x < Grid && y >= 0 && y < Grid)
            {
                switch (Rotation)
                {
                    case 0:
                        if (x + Ships[ShipPointer].Size > Grid) return false;
                        else return true;
                    case 1:
                        if (y + Ships[ShipPointer].Size > Grid) return false;
                        else return true;
                    default:
                        throw new InvalidDataException($"Invalid data at ShipInBounds(): \"{Rotation}\" is not a valid value, expected \"horizontal\" or \"vertical\"");
                }
            }
            else
            {
                return false;
            }
        }
    }
}
