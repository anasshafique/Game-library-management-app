using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007
{
    /// <summary>
    /// Player class - inherits from Person
    /// Now contains a collection of games with individual statistics
    /// Demonstrates inheritance, composition, and nested collections
    /// </summary>
    public class Player : Person
    {
        private List<Game> games;

        public List<Game> Games
        {
            get { return games; }
            set { games = value; }
        }

        // Computed properties for backward compatibility and aggregation
        public double TotalHoursPlayed
        {
            get { return games != null && games.Count > 0 ? games.Sum(g => g.HoursPlayed) : 0; }
        }

        public int HighestScore
        {
            get { return games != null && games.Count > 0 ? games.Max(g => g.HighScore) : 0; }
        }

        // For JSON file because it cannot work with parameterized constructors
        public Player()
        {
            games = new List<Game>();
        }

        // Constructor
        public Player(int id, string name) : base(id, name)
        {
            games = new List<Game>();
        }

        /// <summary>
        /// Add a new game to the player's library
        /// </summary>
        public void AddGame(string gameName)
        {
            if (string.IsNullOrWhiteSpace(gameName))
                throw new ArgumentException("Game name cannot be empty.");

            // Check if game already exists
            if (games.Any(g => g.GameName.Equals(gameName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Game '{gameName}' already exists for this player.");
            }

            Game newGame = new Game(gameName);
            games.Add(newGame);
            Logger.GetInstance().Log($"Added game '{gameName}' to player {UserName}");
        }

        /// <summary>
        /// Get a specific game by name
        /// </summary>
        public Game GetGame(string gameName)
        {
            return games.FirstOrDefault(g => g.GameName.Equals(gameName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Add hours to a specific game
        /// </summary>
        public void AddHoursToGame(string gameName, double hours)
        {
            Game game = GetGame(gameName);
            if (game == null)
                throw new InvalidOperationException($"Game '{gameName}' not found for player {UserName}");

            game.AddHours(hours);
        }

        /// <summary>
        /// Update high score for a specific game
        /// </summary>
        public void UpdateGameHighScore(string gameName, int score)
        {
            Game game = GetGame(gameName);
            if (game == null)
                throw new InvalidOperationException($"Game '{gameName}' not found for player {UserName}");

            game.UpdateHighScore(score);
        }

        /// <summary>
        /// Get all games for this player
        /// </summary>
        public List<Game> GetAllGames()
        {
            return new List<Game>(games);
        }

        // Override ToString for polymorphism demonstration
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{base.ToString()}");
            sb.AppendLine($"Total Hours Played: {TotalHoursPlayed:F2}");
            sb.AppendLine($"Highest Score: {HighestScore}");
            sb.AppendLine($"Games Played: {games.Count}");
            
            if (games.Count > 0)
            {
                sb.AppendLine("Game Details:");
                foreach (Game game in games)
                {
                    sb.AppendLine($"  - {game.ToString()}");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get a summary without game details
        /// </summary>
        public string ToSummaryString()
        {
            return $"{base.ToString()}, Total Hours: {TotalHoursPlayed:F2}, Highest Score: {HighestScore}, Games: {games.Count}";
        }
    }
}
