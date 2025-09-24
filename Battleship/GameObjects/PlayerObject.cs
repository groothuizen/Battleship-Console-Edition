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

        /// <summary>
        /// The current status of the attack from the enum: "AttackStatuses". <br/>
        /// <br/>
        /// Default: AttackStatuses.STANDBY
        /// </summary>
        public AttackStatuses AttackStatus { get; set; } = AttackStatuses.STANDBY;

        public string Name;
        public int Points = 0;
        public int SunkenShips = 0;

        public BoardObject Board { get; private set; } = new();
        public FleetObject Fleet { get; private set; } = new();

        /// <summary>
        /// Keeps track of the amount of ships placed.
        /// </summary>
        public int ShipsPlaced = 0;

        /// <summary>
        /// Attacks a target player and flips values on their board accordingly. <br/>
        /// <br/>
        /// Alters the status of the attack after assessement, example: "AttackStatuses.HIT".
        /// </summary>
        /// <param name="targetPlayer">The player that is being attacked</param>
        /// <param name="x">The x coordinate on the board</param>
        /// <param name="y">The y coordinate on the board</param>
        public void Attack(PlayerObject targetPlayer,int x, int y)
        {
            targetPlayer.Board.FlipValue(x, y, out bool flipped);
            if (flipped)
            {
                if (targetPlayer.Board.Coords[x, y] == BoardStatuses.GUESSED_AND_HIT)
                {
                    var ship = targetPlayer.Fleet.FindShipFromPosition(x, y);
                    if (ship != null)
                    {
                        AttackStatus = AttackStatuses.HIT;

                        ship.Positions[x, y] = 3;
                        ship.Health--;
                        if (ship.Health == 0)
                        {
                            Points++;
                            targetPlayer.SunkenShips++;
                            AttackStatus = AttackStatuses.SUNK;
                        }
                    }
                }
                else if (targetPlayer.Board.Coords[x, y] == BoardStatuses.GUESSED)
                {
                    AttackStatus = AttackStatuses.MISS;
                }
            }
            else AttackStatus = AttackStatuses.ERROR;
        }

        /// <summary>
        /// Checks if the player has won.
        /// </summary>
        /// <returns>true or false</returns>
        public bool HasWon()
        {
            if (Points >= Fleet.Ships.Length) return true;
            else return false;
        }

        /// <summary>
        /// Assess if a ship fits in the given position and then places the ship in the corresponding positions, according to the rotation and size of the ship.
        /// </summary>
        /// <param name="x">The x coordinate on the board</param>
        /// <param name="y">The x coordinate on the board</param>
        /// <exception cref="InvalidDataException"></exception>
        public void PlaceShip(int x, int y)
        {
            switch (Fleet.Rotation)
            {
                case Rotations.HORIZONTAL:
                    if (ShipFits(x, y))
                    {
                        for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                        {
                            Board.Coords[x + i, y] = BoardStatuses.OCCUPIED;
                            Fleet.GetCurrentShip().Positions[x + i, y] = 1;
                        }
                        Fleet.GetCurrentShip().IsPlaced = true;
                        ShipsPlaced++;

                    }
                    break;
                case Rotations.VERTICAL:
                    if (ShipFits(x, y))
                    {
                        for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                        {
                            Board.Coords[x, y + i] = BoardStatuses.OCCUPIED;
                            Fleet.GetCurrentShip().Positions[x + i, y] = 1;
                        }
                        Fleet.GetCurrentShip().IsPlaced = true;
                        ShipsPlaced++;
                    }
                    break;
                default:
                    throw new InvalidDataException($"Invalid data at PlaceShip(): \"{Fleet.Rotation}\" is not a valid value, expected \"Rotations.HORIZONTAL\" or \"Rotations.VERTICAL\"");
            }
        }

        /// <summary>
        /// Assess if a ship fits in the given position and then places the ship in the corresponding positions, according to the rotation and size of the ship.
        /// </summary>
        /// <param name="x">The x coordinate on the board</param>
        /// <param name="y">The x coordinate on the board</param>
        /// <param name="placed">returns true or false, depending on if the ship was successfully placed or not.</param>
        /// <exception cref="InvalidDataException"></exception>
        public void PlaceShip(int x, int y, out bool placed)
        {
            placed = false;
            switch (Fleet.Rotation)
            {
                case Rotations.HORIZONTAL:
                    if (ShipFits(x, y))
                    {
                        for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                        {
                            Board.Coords[x + i, y] = BoardStatuses.OCCUPIED;
                            Fleet.GetCurrentShip().Positions[x + i, y] = 1;
                        }
                        Fleet.GetCurrentShip().IsPlaced = true;
                        ShipsPlaced++;
                        placed = true;
                    }
                    break;
                case Rotations.VERTICAL:
                    if (ShipFits(x, y))
                    {
                        for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                        {
                            Board.Coords[x, y + i] = BoardStatuses.OCCUPIED;
                            Fleet.GetCurrentShip().Positions[x, y + i] = 1;
                        }
                        Fleet.GetCurrentShip().IsPlaced = true;
                        ShipsPlaced++;
                        placed = true;
                    }
                    break;
                default:
                    throw new InvalidDataException($"Invalid data at PlaceShip(): \"{Fleet.Rotation}\" is not a valid value, expected \"Rotations.HORIZONTAL\" or \"Rotations.VERTICAL\"");
            }
        }

        /// <summary>
        /// Assess if a ship fits in the given position, according to the rotation and size of the ship.
        /// </summary>
        /// <param name="x">The x coordinate on the board</param>
        /// <param name="y">The x coordinate on the board</param>
        /// <returns>true or false</returns>
        /// <exception cref="InvalidDataException"></exception>
        public bool ShipFits(int x, int y)
        {
            switch (Fleet.Rotation)
            {
                case Rotations.HORIZONTAL:
                    for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                    {
                        if (Board.Coords[x + i, y] == BoardStatuses.OCCUPIED)
                        {
                            return false;
                        }
                    }
                    break;
                case Rotations.VERTICAL:
                    for (int i = 0; i < Fleet.GetCurrentShip().Size; i++)
                    {
                        if (Board.Coords[x, y + i] == BoardStatuses.OCCUPIED)
                        {
                            return false;
                        }
                    }
                    break;
                default:
                    throw new InvalidDataException($"Invalid data at ShipFits(): \"{Fleet.Rotation}\" is not a valid value, expected \"Rotations.HORIZONTAL\" or \"Rotations.VERTICAL\"");
            }
            return true;
        }

        /// <summary>
        /// Resets the player to the default state.
        /// </summary>
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
