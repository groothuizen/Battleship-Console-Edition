using Battleship.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Scenes
{
    internal class SetupScene : Scene
    {
        public SetupScene() 
        {
            Players = [
                new PlayerObject("Player 1"),
                new PlayerObject("Player 2")
            ];
        }

        private string DialogMessage = string.Empty;

        internal enum PlayerTurnStates
        {
            PLAYER1,
            PLAYER2
        }

        internal PlayerTurnStates PlayerTurn = PlayerTurnStates.PLAYER1;

        public override void Update()
        {
            DialogMessage = String.Empty;
            var CurrentPlayer = Players[(int)PlayerTurn];

            switch (KeyboardHandler.RequestInput())
            {
                case ConsoleKey.UpArrow:
                    if (CurrentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1] - 1))
                    {
                        Cursor.Move("up");
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (CurrentPlayer.Fleet.ShipInBounds(Cursor.Position[0] + 1, Cursor.Position[1]))
                    {
                        Cursor.Move("right");
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (CurrentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1] + 1))
                    {
                        Cursor.Move("down");
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (CurrentPlayer.Fleet.ShipInBounds(Cursor.Position[0] - 1, Cursor.Position[1]))
                    {
                        Cursor.Move("left");
                    }
                    break;
                case ConsoleKey.A:
                    CurrentPlayer.PlaceShip(Cursor.Position[0], Cursor.Position[1], out bool placed);
                    if (placed)
                    {
                        if (CurrentPlayer.ShipsPlaced < CurrentPlayer.Fleet.Ships.Length)
                        {
                            CurrentPlayer.Fleet.ShiftShipPointer();

                            if (!CurrentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1]))
                            {
                                Cursor.PushCursorInBounds(CurrentPlayer.Fleet.Rotation, CurrentPlayer.Fleet.GetCurrentShip().Size);
                            }
                        }
                        else
                        {
                            SwitchPlayers();
                        }
                    }
                    else
                    {
                        DialogMessage = "This area already contains a ship!";
                    }
                    break;
                case ConsoleKey.S:
                    CurrentPlayer.Fleet.ShiftShipPointer();

                    if (!CurrentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1]))
                    {
                        Cursor.PushCursorInBounds(CurrentPlayer.Fleet.Rotation, CurrentPlayer.Fleet.GetCurrentShip().Size);
                    }
                    break;
                case ConsoleKey.D:
                    CurrentPlayer.Fleet.Rotate();

                    if (!CurrentPlayer.Fleet.ShipInBounds(Cursor.Position[0], Cursor.Position[1]))
                    {
                        Cursor.PushCursorInBounds(CurrentPlayer.Fleet.Rotation, CurrentPlayer.Fleet.GetCurrentShip().Size);
                    }
                    break;
                case ConsoleKey.R:
                    CurrentPlayer.Reset();
                    break;
            }
        }

        public override void Draw()
        {
            var CurrentPlayer = Players[(int)PlayerTurn];

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

                                if (CurrentPlayer.Board.Values[x / GridGapX - 1, y / GridGapY - 1] == 1)
                                {
                                    var ship = CurrentPlayer.Fleet.FindShipFromPosition(x / GridGapX - 1, y / GridGapY - 1);

                                    if (ship != null)
                                    {
                                        Console.Write(ship.Symbol);
                                    }
                                }
                                else
                                {
                                    // ===== Cursor Symbol =====

                                    switch (CurrentPlayer.Fleet.Rotation)
                                    {
                                        case 0:
                                            if (Cursor.Position[1] + 1 == y / GridGapY)
                                            {
                                                if
                                                (
                                                    Cursor.Position[0] + 1 == x / GridGapX
                                                    ||
                                                    x / GridGapX > Cursor.Position[0] + 1
                                                    &&
                                                    x / GridGapX <= Cursor.Position[0] + 1 + (CurrentPlayer.Fleet.GetCurrentShip().Size - 1)
                                                )
                                                {
                                                    if (CurrentPlayer.Fleet.GetCurrentShip().Symbol != String.Empty)
                                                    {
                                                        Console.Write(CurrentPlayer.Fleet.GetCurrentShip().Symbol);
                                                    }
                                                    else Console.Write("~");
                                                }
                                                else Console.Write("~");
                                            }
                                            else Console.Write("~");
                                            break;
                                        case 1:
                                            if (Cursor.Position[0] + 1 == x / GridGapX)
                                            {
                                                if
                                                (
                                                    Cursor.Position[1] + 1 == y / GridGapY
                                                    ||
                                                    y / GridGapY > Cursor.Position[1] + 1
                                                    &&
                                                    y / GridGapY <= Cursor.Position[1] + 1 + (CurrentPlayer.Fleet.GetCurrentShip().Size - 1)
                                                )
                                                {
                                                    if (CurrentPlayer.Fleet.GetCurrentShip().Symbol != String.Empty)
                                                    {
                                                        Console.Write(CurrentPlayer.Fleet.GetCurrentShip().Symbol);
                                                    }
                                                    else Console.Write("~");
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
                                switch (CurrentPlayer.Fleet.Rotation)
                                {
                                    case 0:
                                        if (Cursor.Position[1] + 1 == y / GridGapY)
                                        {
                                            if
                                            (
                                                Cursor.Position[0] + 1 == Math.Round(new decimal(x) / GridGapX)
                                                ||
                                                Math.Round(new decimal(x) / GridGapX) > Cursor.Position[0] + 1
                                                &&
                                                Math.Round(new decimal(x) / GridGapX) <= Cursor.Position[0] + 1 + (CurrentPlayer.Fleet.GetCurrentShip().Size - 1)
                                            )
                                            {
                                                Console.Write("|");
                                            }
                                            else Console.Write(" ");
                                        }
                                        else Console.Write(" ");
                                        break;
                                    case 1:
                                        if (Cursor.Position[0] + 1 == Math.Round(new decimal(x) / GridGapX))
                                        {
                                            if
                                            (
                                                Cursor.Position[1] + 1 == y / GridGapY
                                                ||
                                                y / GridGapY > Cursor.Position[1] + 1
                                                &&
                                                y / GridGapY <= Cursor.Position[1] + 1 + (CurrentPlayer.Fleet.GetCurrentShip().Size - 1)
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
                            Console.Write(WhitespaceAround($"Current Turn: {CurrentPlayer.Name}", DialogWidth));
                            break;
                        case 3:
                            Console.Write(WhitespaceAround($"Current Ship: {CurrentPlayer.Fleet.GetCurrentShip().Name} ({CurrentPlayer.Fleet.GetCurrentShip().Symbol}), Size: {CurrentPlayer.Fleet.GetCurrentShip().Size}", DialogWidth));
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
                Cursor.Symbol = Players[(int)PlayerTurn].Fleet.GetCurrentShip().Symbol;
            }
            else if (PlayerTurn == PlayerTurnStates.PLAYER2)
            {
                PlayerTurn = PlayerTurnStates.PLAYER1;
                RequestedScene = new BattleScene();
            }
        }
    }
}
