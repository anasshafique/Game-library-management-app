using Microsoft.VisualStudio.TestTools.UnitTesting;
using CET2007;
using System;

namespace CET2007.Tests
{
    /// <summary>
    /// Unit tests for Game class
    /// Tests game creation, stat updates, and validation
    /// </summary>
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void Game_Constructor_CreatesGameWithZeroStats()
        {
            // Arrange & Act
            Game game = new Game("Minecraft");

            // Assert
            Assert.AreEqual("Minecraft", game.GameName);
            Assert.AreEqual(0, game.HoursPlayed);
            Assert.AreEqual(0, game.HighScore);
        }

        [TestMethod]
        public void Game_ConstructorWithStats_CreatesGameWithProvidedStats()
        {
            // Arrange & Act
            Game game = new Game("Minecraft", 10.5, 100);

            // Assert
            Assert.AreEqual("Minecraft", game.GameName);
            Assert.AreEqual(10.5, game.HoursPlayed);
            Assert.AreEqual(100, game.HighScore);
        }

        [TestMethod]
        public void Game_AddHours_IncreasesHoursPlayed()
        {
            // Arrange
            Game game = new Game("Minecraft");

            // Act
            game.AddHours(5.5);

            // Assert
            Assert.AreEqual(5.5, game.HoursPlayed);
        }

        [TestMethod]
        public void Game_AddHours_AccumulatesHours()
        {
            // Arrange
            Game game = new Game("Minecraft");
            game.AddHours(5.5);

            // Act
            game.AddHours(3.25);

            // Assert
            Assert.AreEqual(8.75, game.HoursPlayed, 0.01);
        }

        [TestMethod]
        public void Game_AddHours_ThrowsExceptionForNegativeHours()
        {
            // Arrange
            Game game = new Game("Minecraft");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => game.AddHours(-5));
        }

        [TestMethod]
        public void Game_AddHours_ThrowsExceptionForZeroHours()
        {
            // Arrange
            Game game = new Game("Minecraft");

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => game.AddHours(0));
        }

        [TestMethod]
        public void Game_UpdateHighScore_UpdatesWhenScoreIsHigher()
        {
            // Arrange
            Game game = new Game("Minecraft");
            game.UpdateHighScore(100);

            // Act
            game.UpdateHighScore(150);

            // Assert
            Assert.AreEqual(150, game.HighScore);
        }

        [TestMethod]
        public void Game_UpdateHighScore_DoesNotUpdateWhenScoreIsLower()
        {
            // Arrange
            Game game = new Game("Minecraft");
            game.UpdateHighScore(100);

            // Act
            game.UpdateHighScore(50);

            // Assert
            Assert.AreEqual(100, game.HighScore);
        }

        [TestMethod]
        public void Game_Constructor_ThrowsExceptionForEmptyName()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new Game(""));
        }

        [TestMethod]
        public void Game_Constructor_ThrowsExceptionForNullName()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new Game(null));
        }

        [TestMethod]
        public void Game_ConstructorWithStats_ThrowsExceptionForNegativeHours()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new Game("Minecraft", -5, 100));
        }

        [TestMethod]
        public void Game_ConstructorWithStats_ThrowsExceptionForNegativeScore()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() => new Game("Minecraft", 10, -100));
        }

        [TestMethod]
        public void Game_ToString_ReturnsFormattedString()
        {
            // Arrange
            Game game = new Game("Minecraft", 10.5, 100);

            // Act
            string result = game.ToString();

            // Assert
            Assert.IsTrue(result.Contains("Minecraft"));
            Assert.IsTrue(result.Contains("10.50"));
            Assert.IsTrue(result.Contains("100"));
        }
    }
}
