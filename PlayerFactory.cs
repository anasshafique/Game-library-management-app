using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007
{
    
    /// Factory Pattern for creating Player objects
    /// Centralizes object creation logic
    
    public class PlayerFactory
    {
        private static int nextId = 1;

        /// <summary>
        /// Create a new player with auto-generated ID
        /// </summary>
        public static Player CreatePlayer(string username)
        {
            Player player = new Player(nextId++, username);
            Logger.GetInstance().Log($"Factory created new player: {username} with ID {player.PlayerID}");
            return player;
        }

        /// <summary>
        /// Create a new player with specified ID
        /// </summary>
        public static Player CreatePlayer(int id, string username)
        {
            if (id >= nextId)
                nextId = id + 1;
            
            Player player = new Player(id, username);
            Logger.GetInstance().Log($"Factory created player: {username} with ID {id}");
            return player;
        }

        /// <summary>
        /// Create a player with initial stats for a default game
        /// </summary>
        public static Player CreatePlayerWithStats(int id, string username, double hours, int score)
        {
            if (id >= nextId)
                nextId = id + 1;

            Player player = new Player(id, username);
            
            // Create a default game with the provided stats
            Game defaultGame = new Game("Default Game", hours, score);
            player.Games.Add(defaultGame);
            
            Logger.GetInstance().Log($"Factory created player with stats: {username} (ID: {id}, Hours: {hours}, Score: {score})");
            return player;
        }

        /// <summary>
        /// Create a player with stats for a specific game
        /// </summary>
        public static Player CreatePlayerWithGame(int id, string username, string gameName, double hours, int score)
        {
            if (id >= nextId)
                nextId = id + 1;

            Player player = new Player(id, username);
            
            // Create a game with the provided stats
            Game game = new Game(gameName, hours, score);
            player.Games.Add(game);
            
            Logger.GetInstance().Log($"Factory created player with game: {username} (ID: {id}, Game: {gameName}, Hours: {hours}, Score: {score})");
            return player;
        }

        /// <summary>
        /// Reset the ID counter (useful for testing)
        /// </summary>
        public static void ResetIdCounter()
        {
            nextId = 1;
        }
    }
}
