using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007
{
    /// <summary>
    /// MenuManager encapsulates all menu and UI logic
    /// Demonstrates proper encapsulation - keeps Program.cs minimal
    /// </summary>
    internal class MenuManager
    {
        private PlayerManager playerManager;

        public MenuManager()
        {
            playerManager = new PlayerManager();
        }

        /// <summary>
        /// Display application title
        /// </summary>
        private void ShowTitle()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("  Game Library & Player Stats Manager");
            Console.WriteLine("========================================");
            Console.WriteLine();
        }

        /// <summary>
        /// Display main menu options
        /// </summary>
        private void ShowMenu()
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("1. Add New Player");
            Console.WriteLine("2. View All Players");
            Console.WriteLine("3. Search Player");
            Console.WriteLine("4. Update Player Stats");
            Console.WriteLine("5. Reports");
            Console.WriteLine("6. Save Data");
            Console.WriteLine("7. Exit");
            Console.Write("\nEnter your choice: ");
        }

        /// <summary>
        /// Process user menu choice
        /// </summary>
        private void ProcessChoice(string choice)
        {
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    playerManager.AddPlayer();
                    break;

                case "2":
                    playerManager.ViewAllPlayers();
                    break;

                case "3":
                    playerManager.SearchPlayer();
                    break;

                case "4":
                    playerManager.UpdatePlayerStats();
                    break;

                case "5":
                    playerManager.ShowReports();
                    break;

                case "6":
                    playerManager.SaveData();
                    break;

                case "7":
                    Console.WriteLine("Saving data and exiting...");
                    playerManager.SaveData();
                    Logger.GetInstance().Log("Application exited by user");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Logger.GetInstance().Log($"Invalid menu choice: {choice}");
                    break;
            }
        }

        /// <summary>
        /// Main application loop
        /// </summary>
        public void Run()
        {
            ShowTitle();
            Logger.GetInstance().Log("Application started");

            while (true)
            {
                try
                {
                    ShowMenu();
                    string choice = Console.ReadLine();
                    ProcessChoice(choice);

                    if (choice != "7")
                    {
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        ShowTitle();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nUnexpected error: {ex.Message}");
                    Logger.GetInstance().Log($"Unexpected error in main loop: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
}
