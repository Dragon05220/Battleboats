using System;
using System.Collections;
using System.IO;
using System.IO.Enumeration;

namespace Battleboats
{
    class Program
    {
        // Defines grids as a global var, easier for all subs to access
        static int[,] playerGrid = { { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };
        static int[,] botGrid = { { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };
        static int playerHitCount = 0;
        static int botHitCount = 0;

        static void Main(string[] args)
        {
            string UserChoice;

            Console.WriteLine("Welcome to Battleboats, Commander.");

            
            // Loops menu until manually quit
            while (true)
            {
                MenuDisplay();
                UserChoice = Console.ReadLine();
                switch (UserChoice)
                 {
                    case "1":
                        playGame();
                        break;
                    case "2":
                        LoadInstructions();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "3":
                        System.Environment.Exit(1967);
                        break;
                }
            }
        }

        
        // Displays menu
        static void MenuDisplay()
        {
            Console.Write($"1. New game\n2. Read Instructions\n3. Quit game\n> ");
        }
        
        // Calls all associated functions
        static void playGame()
        {
            bool playing = true;

            Console.Clear();
            playerGridDisplay();
            aiGridDisplay();
            shipPlace();
            aiShipPlace();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("TARGETING SYSTEM ONLINE\n\n");
            while ((playing) || (botHitCount == 5) || (playerHitCount == 5))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("TARGETING SYSTEM ACTIVE");
                Console.ForegroundColor = ConsoleColor.White;
                // Verifies correct input
                playerAttack();
                if ((botHitCount == 5) || (playerHitCount == 5))
                {
                    break;
                }
                aiAttack();
            }
            // Determines who won
            if (botHitCount == 5)
            {
                Console.WriteLine("The enemy has sunk all our ships.");
                System.Environment.Exit(1967);
            }
            else
            {
                Console.WriteLine("We have destroyed the enemy.");
                System.Environment.Exit(1967);
            }
        }
        // Displays instructions
        static void LoadInstructions()
        {
            Console.WriteLine("1. Enter 5 coordinates for your boats. \n2. Enter attack coordinates. \n3. Press ENTER for both grids which come up. \n4. Repeat until one side wins.");
        }
        //Displays player's grid
        static void playerGridDisplay()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    1  2  3  4  5  6  7  8");
            for (int n = 0; n < 8; n++)
            {
                for (int m = 0; m < 8; m++)
                {
                    if (m == 0)
                    {
                        Console.Write($" {n + 1} ");
                    }
                    Console.Write("[");
                    if (playerGrid[m, n] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(".");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (playerGrid[m, n] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("B");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (playerGrid[m, n] == 2)
                    {
                        Console.Write("x");
                    }
                    else if (playerGrid[m, n] == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    Console.Write("]");
                }
                Console.Write("\n");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        // Displays bot's grid
        static void aiGridDisplay()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"    1  2  3  4  5  6  7  8");
            for (int n = 0; n < 8; n++)
            {
                for (int m = 0; m < 8; m++)
                {
                    if (m == 0)
                    {
                        Console.Write($" {n + 1} ");
                    }
                    Console.Write("[");
                    if ((botGrid[m, n] == 0) || (botGrid[m, n] == 1))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(".");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (botGrid[m, n] == 2)
                    {
                        Console.Write("x");
                    }
                    else if (botGrid[m, n] == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write("]");
                }
                Console.Write("\n");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
        // Ship placement
        static void shipPlace()
        {
            int xCoord = 0;
            int yCoord = 0;

            shipPlacement(xCoord, yCoord);
            shipPlacement(xCoord, yCoord);
            shipPlacement(xCoord, yCoord);
            shipPlacement(xCoord, yCoord);
            shipPlacement(xCoord, yCoord);
        }
        // Places bot ships
        static void aiShipPlace()
        {
            int xCoord;
            int yCoord;

            Random rnd = new Random();
            for (int n = 0; n < 5; n++)
            {
                xCoord = rnd.Next(7);
                yCoord = rnd.Next(7);
                botGrid[xCoord, yCoord] = 1;
            }
        }
        //Ship Placement Function
        static void shipPlacement(int xCoord, int yCoord)
        {
            bool correct = false;

            while (correct == false)
            {
                Console.WriteLine("Please enter the x coordinates of the Destroyer.");
                xCoord = Convert.ToInt32(Console.ReadLine()) - 1;

                Console.WriteLine("Please enter the y coordinates of the Destroyer.");
                yCoord = Convert.ToInt32(Console.ReadLine()) - 1;

                if (playerGrid[xCoord, yCoord] == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nError detected. Please check input.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                } else if ((xCoord <= -1) || (xCoord > 8))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nError detected. Please check input.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                } else if ((yCoord <= -1) || (yCoord > 8)) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nError detected. Please check input.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    correct = true;
                    playerGrid[xCoord, yCoord] = 1;
                    Console.Clear();
                    playerGridDisplay();
                }
                
            }
        }
        // Attack enemy grid
        static void playerAttack()
        {
            int xAttack;
            int yAttack;
            bool correct = false;

            aiGridDisplay();
            while (correct == false)
            {
                Console.Write("Target X-Coordinate: ");
                xAttack = Convert.ToInt32(Console.ReadLine()) - 1;

                Console.Write("\nTarget Y-Coordinate: ");
                yAttack = Convert.ToInt32(Console.ReadLine()) - 1;

                if (botGrid[xAttack, yAttack] == 0)
                {
                    botGrid[xAttack, yAttack] = 2;
                    Console.Clear();
                    aiGridDisplay();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error. Target not found.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    correct = true;
                }
                else if (botGrid[xAttack, yAttack] == 1)
                {
                    botGrid[xAttack, yAttack] = 3;

                    Console.Clear();
                    aiGridDisplay();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Target located.\nTarget confirmed.\nVLS loaded.\nFiring.\nTarget hit.\nTarget destroyed.");
                    Console.ForegroundColor = ConsoleColor.White;
                    playerHitCount++;
                    correct = true;
                }
                else if ((xAttack <= -1) || (xAttack > 8))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nError detected. Please check input.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if ((yAttack <= -1) || (yAttack > 8))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nError detected. Please check input.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
            Console.ReadKey();
        }
        // Attacks player grid, random attack
        static void aiAttack()
        {
            bool correct = false;
            int xAttack;
            int yAttack;
            
            Random rnd = new Random();
            
            while (correct == false)
            {
                xAttack = rnd.Next(7);
                yAttack = rnd.Next(7);

                Console.Clear();
                if (playerGrid[xAttack, yAttack] == 0)
                {
                    playerGrid[xAttack, yAttack] = 2;
                    Console.WriteLine($"Notification: Grid [{xAttack+1}, {yAttack+1}] has been struck.");
                    correct = true;
                } else if (playerGrid[xAttack, yAttack] == 1)
                {
                    playerGrid[xAttack, yAttack] = 3;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Alert: Destroyer [DDG-{rnd.Next(999)}] has been sunk.");
                    Console.ForegroundColor = ConsoleColor.White;
                    correct = true;
                    botHitCount++;
                } 
            }
            playerGridDisplay();
            Console.ReadKey();
            Console.Clear();
        }
    }
}


/* 0 Means blank cell
 * 1 Means a boat is present
 * 2 Means a blank cell has been hit.
 * 3 Means a boat has been hit.
 */

