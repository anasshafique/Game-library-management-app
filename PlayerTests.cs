using Microsoft.VisualStudio.TestTools.UnitTesting;
using CET2007;
using System;

namespace CET2007.Tests
{
    /// <summary>
    /// Unit tests for Player class
    /// Tests player creation, game management, and statistics
    /// </summary>
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void Player_Constructor_CreatesPlayerWithEmptyGamesList()
        {
            // Arrange & Act
            Player player = new Player(1, "TestPlayer");

            // Assert
            Assert.IsNotNull(player.Games);
            Assert.AreEqual(0, player.Games.Count);
            Assert.AreEqual(1, player.PlayerID);
            Assert.AreEqual("TestPlayer", player.UserName);
        }

        [TestMethod]
        public void Player_TotalHoursPlayed_ReturnsZeroWhenNoGames()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");

            // Act
            double totalHours = player.TotalHoursPlayed;

            // Assert
            Assert.AreEqual(0, totalHours);
        }

        [TestMethod]
        public void Player_HighestScore_ReturnsZeroWhenNoGames()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");

            // Act
            int highestScore = player.HighestScore;

            // Assert
            Assert.AreEqual(0, highestScore);
        }

        [TestMethod]
        public void Player_AddGame_AddsGameToList()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");

            // Act
            player.AddGame("Minecraft");

            // Assert
            Assert.AreEqual(1, player.Games.Count);
            Assert.AreEqual("Minecraft", player.Games[0].GameName);
        }

        [TestMethod]
        public void Player_AddGame_ThrowsExceptionForDuplicateGame()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");
            player.AddGame("Minecraft");

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => player.AddGame("Minecraft"));
        }

        [TestMethod]
        public void Player_AddHoursToGame_IncreasesHoursForSpecificGame()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");
            player.AddGame("Minecraft");

            // Act
            player.AddHoursToGame("Minecraft", 5.5);

            // Assert
            Game game = player.GetGame("Minecraft");
            Assert.AreEqual(5.5, game.HoursPlayed);
        }

        [TestMethod]
        public void Player_UpdateGameHighScore_UpdatesScoreWhenHigher()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");
            player.AddGame("Minecraft");
            player.UpdateGameHighScore("Minecraft", 100);

            // Act
            player.UpdateGameHighScore("Minecraft", 150);

            // Assert
            Game game = player.GetGame("Minecraft");
            Assert.AreEqual(150, game.HighScore);
        }

        [TestMethod]
        public void Player_UpdateGameHighScore_DoesNotUpdateWhenLower()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");
            player.AddGame("Minecraft");
            player.UpdateGameHighScore("Minecraft", 100);

            // Act
            player.UpdateGameHighScore("Minecraft", 50);

            // Assert
            Game game = player.GetGame("Minecraft");
            Assert.AreEqual(100, game.HighScore);
        }

        [TestMethod]
        public void Player_TotalHoursPlayed_AggregatesAcrossMultipleGames()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");
            player.AddGame("Minecraft");
            player.AddGame("Fortnite");
            player.AddGame("Valorant");

            // Act
            player.AddHoursToGame("Minecraft", 10.5);
            player.AddHoursToGame("Fortnite", 5.25);
            player.AddHoursToGame("Valorant", 3.75);

            // Assert
            Assert.AreEqual(19.5, player.TotalHoursPlayed, 0.01);
        }

        [TestMethod]
        public void Player_HighestScore_ReturnsMaximumAcrossAllGames()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");
            player.AddGame("Minecraft");
            player.AddGame("Fortnite");
            player.AddGame("Valorant");

            // Act
            player.UpdateGameHighScore("Minecraft", 100);
            player.UpdateGameHighScore("Fortnite", 250);
            player.UpdateGameHighScore("Valorant", 150);

            // Assert
            Assert.AreEqual(250, player.HighestScore);
        }

        [TestMethod]
        public void Player_Constructor_ThrowsExceptionForInvalidID()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new Player(-1, "TestPlayer"));
        }

        [TestMethod]
        public void Player_Constructor_ThrowsExceptionForEmptyUsername()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new Player(1, ""));
        }
    }
}
