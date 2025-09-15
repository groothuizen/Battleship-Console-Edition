using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Battleship.Enums;

namespace Battleship.GameObjects
{
    public class PlayerObject : GameObject
    {
        public PlayerObject(string name)
        {
            Name = name;
        }

        public HitStatuses HitStatus { get; set; } = HitStatuses.STANDBY;
        public string HitStatusMessage = String.Empty;

        public string Name;
        public int Points = 0;
        public int SunkenShips = 0;

        public BoardObject Board { get; private set; } = new();
        public FleetObject Fleet { get; private set; } = new();

        public int ShipsPlaced = 0;

        public void Attack(PlayerObject targetPlayer,int x, int y)
        {
            targetPlayer.Board.FlipValue(x, y, out bool flipped);
            if (flipped)
            {
                if (targetPlayer.Board.Values[x, y] == 3)
                {
                    var ship = targetPlayer.Fleet.FindShipFromPosition(x, y);
                    if (ship != null)
                    {
                        HitStatus = HitStatuses.HIT;

                        ship.Positions[x, y] = 3;
                        ship.Health--;
                        if (ship.Health == 0)
                        {
                            Points++;
                            targetPlayer.SunkenShips++;
                            HitStatus = HitStatuses.SUNK;
                        }
                    }
                }
                else if (targetPlayer.Board.Values[x, y] == 2)
                {
                    HitStatus = HitStatuses.MISS;
                }
            }
            else HitStatus = HitStatuses.ERROR;
        }

        public bool HasWon()
        {
            if (Points >= Fleet.Ships.Length) return true;
            else return false;
        }

        public void PlaceShip(int x, int y)
        {
            switch (Fleet.Rotation)
            {
                case 0:
                    if (ShipFits(x, y))
                    {
                        for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                        {
                            Board.Values[x + i, y] = 1;
                            Fleet.GetCurrentShip().Positions[x + i, y] = 1;
                        }
                        Fleet.GetCurrentShip().IsPlaced = true;
                        ShipsPlaced++;
                    }
                    break;
                case 1:
                    if (ShipFits(x, y))
                    {
                        for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                        {
                            Board.Values[x, y + i] = 1;
                            Fleet.GetCurrentShip().Positions[x + i, y] = 1;
                        }
                        Fleet.GetCurrentShip().IsPlaced = true;
                        ShipsPlaced++;
                    }
                    break;
                default:
                    throw new InvalidDataException($"Invalid data at PlaceShip(): \"{Fleet.Rotation}\" is not a valid value, expected \"0\" for horizontal or \"1\" for vertical");
            }
        }

        public void PlaceShip(int x, int y, out bool placed)
        {
            placed = false;
            switch (Fleet.Rotation)
            {
                case 0:
                    if (ShipFits(x, y))
                    {
                        for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                        {
                            Board.Values[x + i, y] = 1;
                            Fleet.GetCurrentShip().Positions[x + i, y] = 1;
                        }
                        Fleet.GetCurrentShip().IsPlaced = true;
                        ShipsPlaced++;
                        placed = true;
                    }
                    break;
                case 1:
                    if (ShipFits(x, y))
                    {
                        for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                        {
                            Board.Values[x, y + i] = 1;
                            Fleet.GetCurrentShip().Positions[x, y + i] = 1;
                        }
                        Fleet.GetCurrentShip().IsPlaced = true;
                        ShipsPlaced++;
                        placed = true;
                    }
                    break;
                default:
                    throw new InvalidDataException($"Invalid data at PlaceShip(): \"{Fleet.Rotation}\" is not a valid value, expected \"0\" for horizontal or \"1\" for vertical");
            }
        }

        public bool ShipFits(int x, int y)
        {
            switch (Fleet.Rotation)
            {
                case 0:
                    for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                    {
                        if (Board.Values[x + i, y] == 1)
                        {
                            return false;
                        }
                    }
                    break;
                case 1:
                    for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                    {
                        if (Board.Values[x, y + i] == 1)
                        {
                            return false;
                        }
                    }
                    break;
                default:
                    throw new InvalidDataException($"Invalid data at ShipFits(): \"{Fleet.Rotation}\" is not a valid value, expected \"0\" for horizontal or \"1\" for vertical");
            }
            return true;
        }

        public void Reset()
        {
            Board = new();
            Fleet = new();
            Points = 0;
            SunkenShips = 0;
            ShipsPlaced = 0;
        }
    }
}
