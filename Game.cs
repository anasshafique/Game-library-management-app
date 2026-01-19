using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007
{
    /// <summary>
    /// Represents a game with its statistics
    /// Demonstrates composition and nested data structures
    /// </summary>
    public class Game
    {
        private string sGameName;
        private double dHoursPlayed;
        private int iHighScore;

        public string GameName
        {
            get { return sGameName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Game name cannot be empty.");
                sGameName = value;
            }
        }

        public double HoursPlayed
        {
            get { return dHoursPlayed; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Hours played cannot be negative.");
                dHoursPlayed = value;
            }
        }

        public int HighScore
        {
            get { return iHighScore; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("High score cannot be negative.");
                iHighScore = value;
            }
        }

        // For JSON deserialization
        public Game() { }

        public Game(string gameName)
        {
            if (string.IsNullOrWhiteSpace(gameName))
                throw new ArgumentException("Game name cannot be empty.");
            
            sGameName = gameName;
            dHoursPlayed = 0;
            iHighScore = 0;
        }

        public Game(string gameName, double hours, int score)
        {
            if (string.IsNullOrWhiteSpace(gameName))
                throw new ArgumentException("Game name cannot be empty.");
            if (hours < 0)
                throw new ArgumentException("Hours played cannot be negative.");
            if (score < 0)
                throw new ArgumentException("High score cannot be negative.");

            sGameName = gameName;
            dHoursPlayed = hours;
            iHighScore = score;
        }

        public void AddHours(double hours)
        {
            if (hours > 0)
            {
                dHoursPlayed += hours;
                Logger.GetInstance().Log($"Added {hours} hours to game '{sGameName}'. Total: {dHoursPlayed}");
            }
            else
            {
                throw new ArgumentException("Hours must be positive.");
            }
        }

        public void UpdateHighScore(int score)
        {
            if (score > iHighScore)
            {
                iHighScore = score;
                Logger.GetInstance().Log($"Updated high score for game '{sGameName}' to {iHighScore}");
            }
        }

        public override string ToString()
        {
            return $"Game: {sGameName}, Hours: {dHoursPlayed:F2}, High Score: {iHighScore}";
        }
    }
}
