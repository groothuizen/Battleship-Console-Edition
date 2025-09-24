using Battleship.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Scenes
{
    internal class BattleScene : Scene
    {
        public BattleScene() 
        {
            Cursor.Symbol = "";
            Cursor.Position[0] = 0;
            Cursor.Position[1] = 0;
        }

        private string DialogMessage = string.Empty;

        internal PlayerTurnStates PlayerTurn = PlayerTurnStates.PLAYER1;

        internal PlayerTurnStates TargetPlayerTurn = PlayerTurnStates.PLAYER2;

        public override void Update()
        {
            var currentPlayer = Players[(int)PlayerTurn];
            var targetPlayer = Players[(int)TargetPlayerTurn];

            switch (KeyboardHandler.RequestInput())
            {
                case ConsoleKey.UpArrow:
                    Cursor.Move("up");
                    break;
                case ConsoleKey.RightArrow:
                    Cursor.Move("right");
                    break;
                case ConsoleKey.DownArrow:
                    Cursor.Move("down");
                    break;
                case ConsoleKey.LeftArrow:
                    Cursor.Move("left");
                    break;
                case ConsoleKey.A:
                    currentPlayer.Attack(targetPlayer, Cursor.Position[0], Cursor.Position[1]);

                    switch (currentPlayer.AttackStatus)
                    {
                        case AttackStatuses.HIT:
                            Draw();
                            Thread.Sleep(2000);
                            break;
                        case AttackStatuses.SUNK:
                            Draw();
                            Thread.Sleep(2000);
                            break;
                        case AttackStatuses.MISS:
                            Draw();
                            Thread.Sleep(2000);
                            SwitchPlayers();
                            break;
                        case AttackStatuses.ERROR:
                            Draw();
                            Thread.Sleep(1000);
                            break;
                    }

                    if (currentPlayer.HasWon())
                    {
                        RequestedScene = new WinScene();
                    }

                    currentPlayer.AttackStatus = AttackStatuses.STANDBY;
                    break;
            }
        }

        // Sorry about the if statement mess, drawing graphics inside a console is hard...
        public override void Draw()
        {
            var currentPlayer = Players[(int)PlayerTurn];
            var targetPlayer = Players[(int)TargetPlayerTurn];

            for (int y = 1; y < Height; y++) // starts counting at 1 to 
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
                            y % GridGapY == 0
                        )
                        {
                            // ===== Grid Symbols =====

                            if (x % GridGapX == 0)
                            {
                                // ===== Ship Symbol =====

                                switch (targetPlayer.Board.Coords[x / GridGapX - 1, y / GridGapY - 1])
                                {
                                    case BoardStatuses.GUESSED:
                                        Console.Write("O");
                                        break;
                                    case BoardStatuses.GUESSED_AND_HIT:
                                        Console.Write("X");
                                        break;
                                    default:
                                        Console.Write("~");
                                        break;
                                }
                            }
                            else if
                            (
                                (x + 1) % GridGapX == 0
                                ||
                                (x - 1) % GridGapX == 0
                            )
                            {
                                if
                                (
                                    Cursor.Position[1] + 1 == y / GridGapY
                                    &&
                                    Cursor.Position[0] + 1 == Math.Round(new decimal(x) / GridGapX)
                                )
                                {
                                    Console.Write("|");
                                }
                                else Console.Write(" ");
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
                int DialogStartPosition = Height / 2 - 5;
                int DialogWidth = 50;
                int DialogHeight = 10;
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
                        switch (y - DialogStartPosition)
                        {
                            case 2:
                                Console.Write(WhitespaceAround($"Current Turn: {currentPlayer.Name}", DialogWidth));
                                break;
                            case 3:
                                Console.Write(WhitespaceAround($"Ships Sunk: {targetPlayer.SunkenShips}", DialogWidth));
                                break;
                            case 5:
                                switch (currentPlayer.AttackStatus)
                                {
                                    case AttackStatuses.STANDBY:
                                        DialogMessage = "Pick a spot to attack!";
                                        break;
                                    case AttackStatuses.MISS:
                                        DialogMessage = "YOU MISSED!";
                                        break;
                                    case AttackStatuses.HIT:
                                        DialogMessage = "A DIRECT HIT!";
                                        break;
                                    case AttackStatuses.SUNK:
                                        DialogMessage = "A SHIP HAS SUNK!";
                                        break;
                                    case AttackStatuses.ERROR:
                                        DialogMessage = "That point has already been hit!";
                                        break;
                                }
                                Console.Write(WhitespaceAround(((DialogMessage != String.Empty) ? $"{DialogMessage}" : ""), DialogWidth));
                                break;
                            case 7:
                                Console.Write(WhitespaceAround("-- CONTROLS --", DialogWidth));
                                break;
                            case 9:
                                Console.Write(WhitespaceAround("Arrow Keys : move cursor | A : shoot", DialogWidth));
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
                TargetPlayerTurn = PlayerTurnStates.PLAYER1;
            }
            else if (PlayerTurn == PlayerTurnStates.PLAYER2)
            {
                PlayerTurn = PlayerTurnStates.PLAYER1;
                TargetPlayerTurn = PlayerTurnStates.PLAYER2;
            }
        }
    }
}
