using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Battleship.Enums;

namespace Battleship.Scenes
{
    internal class SetupScene : Scene
    {
        public SetupScene()
        {
            foreach (var player in Players)
            {
                player.Reset();
            }

            Serilog.Log.Information
            (
                "#SETUP starting the setup scene..."
            );
        }

        private string DialogMessage = string.Empty;

        internal PlayerTurnStates PlayerTurn = PlayerTurnStates.PLAYER1;

        public override void Update()
        {
            DialogMessage = String.Empty;
            var currentPlayer = Players[(int)PlayerTurn];

            switch (KeyboardHandler.RequestInput())
            {
                case ConsoleKey.UpArrow:
                    if (currentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1] - 1))
                    {
                        Cursor.Move("up");
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (currentPlayer.Fleet.ShipInBounds(Cursor.Position[0] + 1, Cursor.Position[1]))
                    {
                        Cursor.Move("right");
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (currentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1] + 1))
                    {
                        Cursor.Move("down");
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (currentPlayer.Fleet.ShipInBounds(Cursor.Position[0] - 1, Cursor.Position[1]))
                    {
                        Cursor.Move("left");
                    }
                    break;
                case ConsoleKey.A:
                    currentPlayer.PlaceShip(Cursor.Position[0], Cursor.Position[1], out bool placed);
                    if (placed)
                    {
                        Serilog.Log.Information
                        (
                            "#SETUP player: {playerName} placed a ship at coordinates: {shipCoordinates}",
                            currentPlayer.Name, currentPlayer.Fleet.GetCurrentShip().Coords
                        );

                        if (currentPlayer.ShipsPlaced < currentPlayer.Fleet.Ships.Length)
                        {
                            currentPlayer.Fleet.ShiftShipPointer();

                            if (!currentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1]))
                            {
                                Cursor.PushCursorInBounds(currentPlayer.Fleet.Rotation, currentPlayer.Fleet.GetCurrentShip().Size);
                            }
                        }
                        else
                        {
                            Serilog.Log.Information
                            (
                                "#SETUP player: {playerName} has placed all their ships! {playerName}'s board status: {boardStatus}",
                                currentPlayer.Name, currentPlayer.Name, currentPlayer.Board.Coords
                            );

                            SwitchPlayers();
                        }
                    }
                    else
                    {
                        DialogMessage = "This area already contains a ship!";
                    }
                    break;
                case ConsoleKey.S:
                    currentPlayer.Fleet.ShiftShipPointer();

                    if (!currentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1]))
                    {
                        Cursor.PushCursorInBounds(currentPlayer.Fleet.Rotation, currentPlayer.Fleet.GetCurrentShip().Size);
                    }
                    break;
                case ConsoleKey.D:
                    currentPlayer.Fleet.Rotate();

                    if (!currentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1]))
                    {
                        Cursor.PushCursorInBounds(currentPlayer.Fleet.Rotation, currentPlayer.Fleet.GetCurrentShip().Size);
                    }
                    break;
                case ConsoleKey.R:
                    currentPlayer.Reset();
                    break;
            }
        }

        public override void Draw()
        {
            var currentPlayer = Players[(int)PlayerTurn];

            for (int y = 1; y < Height; y++) // starts counting at 1 to avoid dividing by 0
            {
                if (y > 1) Console.Write("|"); // LEFTMOST BOUNDARY
                else Console.Write(" ");

                for (int x = 1; x < Width; x++) // starts counting at 1 to avoid dividing by 0
                {
                    if (y == 1) Console.Write("_"); // TOP BOUNDARY
                    else if (y == Height - 1) Console.Write("_"); // BOTTOM BOUNDARY
                    else
                    {
                        if
                        (
                            x > 1
                            &&
                            x < Width - Grid / 2 // revisit this <-----------------------------------------------------------------
                            &&
                            y % GridGapY == 0 // checks if the remainder of the y coordinate divided by the GridGapY value is exactly 0
                        )
                        {

                            // ===== Grid Symbols =====

                            if (x % GridGapX == 0)
                            {
                                // ===== Ship Symbol =====

                                if (currentPlayer.Board.Coords[x / GridGapX - 1, y / GridGapY - 1] == BoardStatuses.OCCUPIED)
                                {
                                    var ship = currentPlayer.Fleet.FindShipFromPosition(x / GridGapX - 1, y / GridGapY - 1);

                                    if (ship != null)
                                    {
                                        Console.Write(ship.Symbol);
                                    }
                                    else
                                    {
                                        Console.Write("?");
                                    }
                                }
                                else
                                {
                                    // ===== Cursor Symbol =====

                                    switch (currentPlayer.Fleet.Rotation)
                                    {
                                        case Rotations.HORIZONTAL:
                                            if (Cursor.Position[1] + 1 == y / GridGapY)
                                            {
                                                if
                                                (
                                                    Cursor.Position[0] + 1 == x / GridGapX
                                                    ||
                                                    x / GridGapX > Cursor.Position[0] + 1
                                                    &&
                                                    x / GridGapX <= Cursor.Position[0] + 1 + (currentPlayer.Fleet.GetCurrentShip().Size - 1)
                                                )
                                                {
                                                    Console.Write(currentPlayer.Fleet.GetCurrentShip().Symbol);
                                                }
                                                else Console.Write("~");
                                            }
                                            else Console.Write("~");
                                            break;
                                        case Rotations.VERTICAL:
                                            if (Cursor.Position[0] + 1 == x / GridGapX)
                                            {
                                                if
                                                (
                                                    Cursor.Position[1] + 1 == y / GridGapY
                                                    ||
                                                    y / GridGapY > Cursor.Position[1] + 1
                                                    &&
                                                    y / GridGapY <= Cursor.Position[1] + 1 + (currentPlayer.Fleet.GetCurrentShip().Size - 1)
                                                )
                                                {
                                                    Console.Write(currentPlayer.Fleet.GetCurrentShip().Symbol);
                                                }
                                                else Console.Write("~");
                                            }
                                            else Console.Write("~");
                                            break;
                                    }
                                }
                            }

                            // ===== Cursor Borders =====

                            else if
                            (
                                (x + 1) % GridGapX == 0
                                ||
                                (x - 1) % GridGapX == 0
                            )
                            {
                                switch (currentPlayer.Fleet.Rotation)
                                {
                                    case Rotations.HORIZONTAL:
                                        if (Cursor.Position[1] + 1 == y / GridGapY)
                                        {
                                            if
                                            (
                                                Cursor.Position[0] + 1 == Math.Round(new decimal(x) / GridGapX)
                                                ||
                                                Math.Round(new decimal(x) / GridGapX) > Cursor.Position[0] + 1
                                                &&
                                                Math.Round(new decimal(x) / GridGapX) <= Cursor.Position[0] + 1 + (currentPlayer.Fleet.GetCurrentShip().Size - 1)
                                            )
                                            {
                                                Console.Write("|");
                                            }
                                            else Console.Write(" ");
                                        }
                                        else Console.Write(" ");
                                        break;
                                    case Rotations.VERTICAL:
                                        if (Cursor.Position[0] + 1 == Math.Round(new decimal(x) / GridGapX))
                                        {
                                            if
                                            (
                                                Cursor.Position[1] + 1 == y / GridGapY
                                                ||
                                                y / GridGapY > Cursor.Position[1] + 1
                                                &&
                                                y / GridGapY <= Cursor.Position[1] + 1 + (currentPlayer.Fleet.GetCurrentShip().Size - 1)
                                            )
                                            {
                                                Console.Write("|");
                                            }
                                            else Console.Write(" ");
                                        }
                                        else Console.Write(" ");
                                        break;
                                }
                            }
                            else Console.Write(" ");
                        }
                        else Console.Write(" ");
                    }
                }
            if (y > 1) Console.Write("|"); // RIGHTMOST BOUNDARY
            else Console.Write(" ");

            Console.Write("    "); // Padding (4 whitespaces)

            //===== Dialog Box =====
            int DialogWidth = 50;
            int DialogHeight = 12;
            int DialogStartPosition = Height / 2 - DialogHeight / 2;
            if (y >= DialogStartPosition && y <= DialogStartPosition + DialogHeight)
            {
                if (y > DialogStartPosition) Console.Write("|"); // LEFTMOST BOUNDARY
                else Console.Write(" ");
                if
                    (
                    y == DialogStartPosition + DialogHeight // TOP BOUNDARY
                    ||
                    y == DialogStartPosition // BOTTOM BOUNDARY
                    )
                {
                    for (int x = 0; x < DialogWidth; x++)
                    {
                        Console.Write("_");
                    }
                }
                else
                {
                    // ===== Dialog Content =====
                    switch (y - DialogStartPosition) // case value is the y coordinate inside the dialog content
                    {
                        case 2:
                            Console.Write(WhitespaceAround($"Current Turn: {currentPlayer.Name}", DialogWidth));
                            break;
                        case 3:
                            Console.Write(WhitespaceAround($"Current Ship: {currentPlayer.Fleet.GetCurrentShip().Name} ({currentPlayer.Fleet.GetCurrentShip().Symbol}), Size: {currentPlayer.Fleet.GetCurrentShip().Size}", DialogWidth));
                            break;
                        case 5:
                            Console.Write(WhitespaceAround(((DialogMessage != String.Empty) ? $"{DialogMessage}" : ""), DialogWidth));
                            break;
                        case 7:
                            Console.Write(WhitespaceAround("-- CONTROLS --", DialogWidth));
                            break;
                        case 9:
                            Console.Write(WhitespaceAround("Arrow Keys : move cursor | R : reset", DialogWidth));
                            break;
                        case 10:
                            Console.Write(WhitespaceAround("A : place ship | S : select next ship", DialogWidth));
                            break;
                        case 11:
                            Console.Write(WhitespaceAround("D : rotate ship", DialogWidth));
                            break;
                        default:
                            for (int x = 0; x < DialogWidth; x++)
                            {
                                Console.Write(" ");
                            }
                            break;
                    }
                }

                if (y > DialogStartPosition) Console.Write("|"); // RIGHTMOST BOUNDARY
                else Console.Write(" ");
            }
                Console.WriteLine();
            }
        }

        private void SwitchPlayers()
        {
            if (PlayerTurn == PlayerTurnStates.PLAYER1)
            {
                PlayerTurn = PlayerTurnStates.PLAYER2;
            }
            else if (PlayerTurn == PlayerTurnStates.PLAYER2)
            {
                PlayerTurn = PlayerTurnStates.PLAYER1;
                RequestedScene = new BattleScene();
            }
        }
    }
}
