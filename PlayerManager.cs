using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CET2007
{
    /// <summary>
    /// PlayerManager implements ISearchable, ISortable, and IReportable interfaces
    /// Manages all player operations including CRUD, search, sort, and reporting
    /// Demonstrates use of interfaces, algorithms, and design patterns
    /// </summary>
    public class PlayerManager : ISearchable, ISortable, IReportable
    {
        private List<Player> listofplayers;
        private const string DATA_FILE = "players.json";

        public PlayerManager()
        {
            // Load existing data using FileManager (Repository Pattern)
            listofplayers = FileManager.LoadFromJson<Player>(DATA_FILE);
            Logger.GetInstance().Log($"PlayerManager initialized with {listofplayers.Count} players");
        }

        #region CRUD Operations

        /// <summary>
        /// Add new players interactively
        /// </summary>
        public void AddPlayer()
        {
            string sChoice;
            string sName;
            int iID;
            int iExit = 1;

            while (iExit != 0)
            {
                try
                {
                    Console.Write("Do you want to add a player? yes/no: ");
                    sChoice = Console.ReadLine();

                    if (sChoice.ToLower() == "yes")
                    {
                        Console.Write("Enter the name of the player: ");
                        sName = Console.ReadLine();
                        
                        if (string.IsNullOrWhiteSpace(sName))
                        {
                            Console.WriteLine("Error: Username cannot be empty.");
                            continue;
                        }

                        Console.Write("Enter the ID: ");
                        if (!int.TryParse(Console.ReadLine(), out iID))
                        {
                            Console.WriteLine("Error: Invalid ID. Please enter a number.");
                            continue;
                        }

                        // Check for duplicate ID
                        if (SearchById(iID) != null)
                        {
                            Console.WriteLine($"Error: Player with ID {iID} already exists.");
                            continue;
                        }

                        // Use Factory Pattern to create player
                        Player newPlayer = PlayerFactory.CreatePlayer(iID, sName);
                        listofplayers.Add(newPlayer);
                        
                        Console.WriteLine($"Player '{sName}' added successfully!");

                        Console.Write("Enter any key to continue or press 0 to exit: ");
                        if (!int.TryParse(Console.ReadLine(), out iExit))
                        {
                            iExit = 1; // Continue if invalid input
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Validation Error: {ex.Message}");
                    Logger.GetInstance().Log($"Validation error in AddPlayer: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Logger.GetInstance().Log($"Error in AddPlayer: {ex.Message}");
                }
            }

            // Save data using FileManager
            SaveData();
        }

        /// <summary>
        /// View all players
        /// </summary>
        public void ViewAllPlayers()
        {
            if (listofplayers.Count == 0)
            {
                Console.WriteLine("No players found in the system.");
                Logger.GetInstance().Log("ViewAllPlayers: No players to display");
                return;
            }

            Console.WriteLine("\n========================================");
            Console.WriteLine("           ALL PLAYERS");
            Console.WriteLine("========================================");
            
            foreach (Player player in listofplayers)
            {
                // Use summary string for list view
                Console.WriteLine(player.ToSummaryString());
                Console.WriteLine("----------------------------------------");
            }

            Console.WriteLine($"Total Players: {listofplayers.Count}");
            Logger.GetInstance().Log($"Displayed {listofplayers.Count} players");
        }

        /// <summary>
        /// Update player statistics - now supports game-specific stats
        /// </summary>
        public void UpdatePlayerStats()
        {
            try
            {
                Console.Write("Enter Player ID to update: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Error: Invalid ID.");
                    return;
                }

                Player player = SearchById(id);
                if (player == null)
                {
                    Console.WriteLine($"Player with ID {id} not found.");
                    return;
                }

                Console.WriteLine($"\nUpdating stats for: {player.UserName}");
                Console.WriteLine($"Total Hours: {player.TotalHoursPlayed:F2}, Highest Score: {player.HighestScore}");
                Console.WriteLine($"Games: {player.Games.Count}");

                // Show current games
                if (player.Games.Count > 0)
                {
                    Console.WriteLine("\nCurrent Games:");
                    for (int i = 0; i < player.Games.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {player.Games[i].ToString()}");
                    }
                }

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add new game");
                Console.WriteLine("2. Update existing game stats");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddGameToPlayer(player);
                        break;

                    case "2":
                        UpdateGameStats(player);
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        return;
                }

                SaveData();
                Console.WriteLine("Player stats updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating player stats: {ex.Message}");
                Logger.GetInstance().Log($"Error in UpdatePlayerStats: {ex.Message}");
            }
        }

        /// <summary>
        /// Add a new game to a player
        /// </summary>
        private void AddGameToPlayer(Player player)
        {
            Console.Write("Enter game name: ");
            string gameName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(gameName))
            {
                Console.WriteLine("Error: Game name cannot be empty.");
                return;
            }

            try
            {
                player.AddGame(gameName);
                Console.WriteLine($"Game '{gameName}' added successfully!");

                // Optionally add initial stats
                Console.Write("Add initial hours? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Write("Enter hours: ");
                    if (double.TryParse(Console.ReadLine(), out double hours) && hours > 0)
                    {
                        player.AddHoursToGame(gameName, hours);
                        Console.WriteLine($"Added {hours} hours to {gameName}");
                    }
                }

                Console.Write("Add initial score? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Write("Enter score: ");
                    if (int.TryParse(Console.ReadLine(), out int score) && score > 0)
                    {
                        player.UpdateGameHighScore(gameName, score);
                        Console.WriteLine($"Set high score to {score} for {gameName}");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update stats for an existing game
        /// </summary>
        private void UpdateGameStats(Player player)
        {
            if (player.Games.Count == 0)
            {
                Console.WriteLine("No games found for this player.");
                return;
            }

            Console.Write("Enter game name: ");
            string gameName = Console.ReadLine();

            Game game = player.GetGame(gameName);
            if (game == null)
            {
                Console.WriteLine($"Game '{gameName}' not found for this player.");
                return;
            }

            Console.WriteLine($"\nCurrent stats for {gameName}:");
            Console.WriteLine($"Hours: {game.HoursPlayed:F2}, High Score: {game.HighScore}");

            Console.Write("Enter hours to add (0 to skip): ");
            if (double.TryParse(Console.ReadLine(), out double hours) && hours > 0)
            {
                player.AddHoursToGame(gameName, hours);
                Console.WriteLine($"Added {hours} hours. New total: {game.HoursPlayed:F2}");
            }

            Console.Write("Enter new high score (0 to skip): ");
            if (int.TryParse(Console.ReadLine(), out int score) && score > 0)
            {
                int oldScore = game.HighScore;
                player.UpdateGameHighScore(gameName, score);
                if (game.HighScore > oldScore)
                {
                    Console.WriteLine($"High score updated to: {game.HighScore}");
                }
                else
                {
                    Console.WriteLine("Score is not higher than current high score.");
                }
            }
        }

        /// <summary>
        /// Save data to file
        /// </summary>
        public void SaveData()
        {
            try
            {
                FileManager.SaveToJson(listofplayers, DATA_FILE);
                Console.WriteLine("Data saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
                Logger.GetInstance().Log($"Error in SaveData: {ex.Message}");
            }
        }

        #endregion

        #region ISearchable Implementation

        /// <summary>
        /// Linear search by ID - O(n) complexity
        /// </summary>
        public Player SearchById(int id)
        {
            Logger.GetInstance().Log($"Linear search for player ID: {id}");
            
            // Linear search algorithm
            for (int i = 0; i < listofplayers.Count; i++)
            {
                if (listofplayers[i].PlayerID == id)
                {
                    Logger.GetInstance().Log($"Player found at index {i}");
                    return listofplayers[i];
                }
            }

            Logger.GetInstance().Log($"Player with ID {id} not found");
            return null;
        }

        /// <summary>
        /// Binary search by ID - O(log n) complexity
        /// Requires sorted list
        /// </summary>
        public Player BinarySearchById(int id)
        {
            Logger.GetInstance().Log($"Binary search for player ID: {id}");

            // First, sort the list by ID
            List<Player> sortedList = listofplayers.OrderBy(p => p.PlayerID).ToList();

            int left = 0;
            int right = sortedList.Count - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (sortedList[mid].PlayerID == id)
                {
                    Logger.GetInstance().Log($"Player found using binary search");
                    return sortedList[mid];
                }

                if (sortedList[mid].PlayerID < id)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            Logger.GetInstance().Log($"Player with ID {id} not found using binary search");
            return null;
        }

        /// <summary>
        /// Search by username - Linear search
        /// </summary>
        public Player SearchByUsername(string username)
        {
            Logger.GetInstance().Log($"Searching for player by username: {username}");

            if (string.IsNullOrWhiteSpace(username))
            {
                Logger.GetInstance().Log("Invalid username provided for search");
                return null;
            }

            // Linear search by username
            foreach (Player player in listofplayers)
            {
                if (player.UserName.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.GetInstance().Log($"Player '{username}' found");
                    return player;
                }
            }

            Logger.GetInstance().Log($"Player '{username}' not found");
            return null;
        }

        /// <summary>
        /// Interactive search menu
        /// </summary>
        public void SearchPlayer()
        {
            try
            {
                Console.WriteLine("\n--- Search Player ---");
                Console.WriteLine("1. Search by ID (Linear)");
                Console.WriteLine("2. Search by ID (Binary)");
                Console.WriteLine("3. Search by Username");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine();
                Player found = null;

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter Player ID: ");
                        if (int.TryParse(Console.ReadLine(), out int id1))
                        {
                            found = SearchById(id1);
                        }
                        break;

                    case "2":
                        Console.Write("Enter Player ID: ");
                        if (int.TryParse(Console.ReadLine(), out int id2))
                        {
                            found = BinarySearchById(id2);
                        }
                        break;

                    case "3":
                        Console.Write("Enter Username: ");
                        string username = Console.ReadLine();
                        found = SearchByUsername(username);
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        return;
                }

                if (found != null)
                {
                    Console.WriteLine("\n--- Player Found ---");
                    Console.WriteLine(found.ToString()); // Shows all game details
                }
                else
                {
                    Console.WriteLine("Player not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during search: {ex.Message}");
                Logger.GetInstance().Log($"Error in SearchPlayer: {ex.Message}");
            }
        }

        #endregion

        #region ISortable Implementation

        /// <summary>
        /// Bubble Sort by Hours Played - Manual implementation
        /// Demonstrates sorting algorithm implementation
        /// </summary>
        public List<Player> SortByHours()
        {
            Logger.GetInstance().Log("Sorting players by hours using Bubble Sort");

            List<Player> sorted = new List<Player>(listofplayers);
            int n = sorted.Count;

            // Bubble Sort Algorithm
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (sorted[j].TotalHoursPlayed > sorted[j + 1].TotalHoursPlayed)
                    {
                        // Swap
                        Player temp = sorted[j];
                        sorted[j] = sorted[j + 1];
                        sorted[j + 1] = temp;
                    }
                }
            }

            Logger.GetInstance().Log("Bubble sort completed");
            return sorted;
        }

        /// <summary>
        /// Sort by Score using LINQ (built-in sort for comparison)
        /// </summary>
        public List<Player> SortByScore()
        {
            Logger.GetInstance().Log("Sorting players by score using LINQ");
            return listofplayers.OrderByDescending(p => p.HighestScore).ToList();
        }

        #endregion

        #region IReportable Implementation

        /// <summary>
        /// Generate top players by score report
        /// </summary>
        public void GenerateTopScoresReport(int count)
        {
            try
            {
                Console.WriteLine("\n========================================");
                Console.WriteLine($"      TOP {count} PLAYERS BY SCORE");
                Console.WriteLine("========================================");

                List<Player> topPlayers = SortByScore().Take(count).ToList();

                if (topPlayers.Count == 0)
                {
                    Console.WriteLine("No players available.");
                    return;
                }

                int rank = 1;
                foreach (Player player in topPlayers)
                {
                    Console.WriteLine($"#{rank} - {player.UserName}");
                    Console.WriteLine($"    Highest Score: {player.HighestScore}, Total Hours: {player.TotalHoursPlayed:F2}, Games: {player.Games.Count}");
                    Console.WriteLine("----------------------------------------");
                    rank++;
                }

                Logger.GetInstance().Log($"Generated top {count} scores report");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating report: {ex.Message}");
                Logger.GetInstance().Log($"Error in GenerateTopScoresReport: {ex.Message}");
            }
        }

        /// <summary>
        /// Generate most active players report
        /// </summary>
        public void GenerateMostActivePlayersReport(int count)
        {
            try
            {
                Console.WriteLine("\n========================================");
                Console.WriteLine($"    TOP {count} MOST ACTIVE PLAYERS");
                Console.WriteLine("========================================");

                List<Player> activePlayers = SortByHours().Reverse<Player>().Take(count).ToList();

                if (activePlayers.Count == 0)
                {
                    Console.WriteLine("No players available.");
                    return;
                }

                int rank = 1;
                foreach (Player player in activePlayers)
                {
                    Console.WriteLine($"#{rank} - {player.UserName}");
                    Console.WriteLine($"    Total Hours: {player.TotalHoursPlayed:F2}, Highest Score: {player.HighestScore}, Games: {player.Games.Count}");
                    Console.WriteLine("----------------------------------------");
                    rank++;
                }

                Logger.GetInstance().Log($"Generated top {count} most active players report");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating report: {ex.Message}");
                Logger.GetInstance().Log($"Error in GenerateMostActivePlayersReport: {ex.Message}");
            }
        }

        /// <summary>
        /// Interactive reports menu
        /// </summary>
        public void ShowReports()
        {
            try
            {
                Console.WriteLine("\n--- Reports ---");
                Console.WriteLine("1. Top Players by Score");
                Console.WriteLine("2. Most Active Players");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine();

                Console.Write("How many players to show? ");
                if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
                {
                    Console.WriteLine("Invalid number.");
                    return;
                }

                switch (choice)
                {
                    case "1":
                        GenerateTopScoresReport(count);
                        break;

                    case "2":
                        GenerateMostActivePlayersReport(count);
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in reports: {ex.Message}");
                Logger.GetInstance().Log($"Error in ShowReports: {ex.Message}");
            }
        }

        #endregion

        /// <summary>
        /// Get player count
        /// </summary>
        public int GetPlayerCount()
        {
            return listofplayers.Count;
        }
    }
}
