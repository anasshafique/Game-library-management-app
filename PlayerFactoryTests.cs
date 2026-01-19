using Microsoft.VisualStudio.TestTools.UnitTesting;
using CET2007;
using System;

namespace CET2007.Tests
{
    /// <summary>
    /// Unit tests for PlayerFactory class
    /// Tests factory pattern implementation and object creation
    /// </summary>
    [TestClass]
    public class PlayerFactoryTests
    {
        [TestInitialize]
        public void Setup()
        {
            PlayerFactory.ResetIdCounter();
        }

        [TestMethod]
        public void CreatePlayer_WithAutoId_CreatesPlayerWithIncrementingId()
        {
            // Act
            Player player1 = PlayerFactory.CreatePlayer("Player1");
            Player player2 = PlayerFactory.CreatePlayer("Player2");

            // Assert
            Assert.AreEqual(1, player1.PlayerID);
            Assert.AreEqual(2, player2.PlayerID);
        }

        [TestMethod]
        public void CreatePlayer_WithSpecifiedId_CreatesPlayerWithGivenId()
        {
            // Act
            Player player = PlayerFactory.CreatePlayer(100, "TestPlayer");

            // Assert
            Assert.AreEqual(100, player.PlayerID);
            Assert.AreEqual("TestPlayer", player.UserName);
        }

        [TestMethod]
        public void CreatePlayer_WithSpecifiedId_UpdatesNextIdCounter()
        {
            // Act
            PlayerFactory.CreatePlayer(100, "Player1");
            Player player2 = PlayerFactory.CreatePlayer("Player2");

            // Assert
            Assert.AreEqual(101, player2.PlayerID);
        }

        [TestMethod]
        public void CreatePlayerWithStats_CreatesPlayerWithDefaultGame()
        {
            // Act
            Player player = PlayerFactory.CreatePlayerWithStats(1, "TestPlayer", 10.5, 100);

            // Assert
            Assert.AreEqual(1, player.Games.Count);
            Assert.AreEqual("Default Game", player.Games[0].GameName);
            Assert.AreEqual(10.5, player.Games[0].HoursPlayed);
            Assert.AreEqual(100, player.Games[0].HighScore);
        }

        [TestMethod]
        public void CreatePlayerWithGame_CreatesPlayerWithSpecificGame()
        {
            // Act
            Player player = PlayerFactory.CreatePlayerWithGame(1, "TestPlayer", "Minecraft", 10.5, 100);

            // Assert
            Assert.AreEqual(1, player.Games.Count);
            Assert.AreEqual("Minecraft", player.Games[0].GameName);
            Assert.AreEqual(10.5, player.Games[0].HoursPlayed);
            Assert.AreEqual(100, player.Games[0].HighScore);
        }

        [TestMethod]
        public void ResetIdCounter_ResetsToOne()
        {
            // Arrange
            PlayerFactory.CreatePlayer("Player1");
            PlayerFactory.CreatePlayer("Player2");

            // Act
            PlayerFactory.ResetIdCounter();
            Player player = PlayerFactory.CreatePlayer("NewPlayer");

            // Assert
            Assert.AreEqual(1, player.PlayerID);
        }
    }
}
