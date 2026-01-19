using Microsoft.VisualStudio.TestTools.UnitTesting;
using CET2007;
using System;
using System.Collections.Generic;
using System.IO;

namespace CET2007.Tests
{
    /// <summary>
    /// Unit tests for FileManager class
    /// Tests JSON persistence and error handling
    /// </summary>
    [TestClass]
    public class FileManagerTests
    {
        private const string TEST_FILE = "test_players.json";

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up test files after each test
            if (File.Exists(TEST_FILE))
            {
                File.Delete(TEST_FILE);
            }
        }

        [TestMethod]
        public void SaveToJson_ValidData_CreatesFile()
        {
            // Arrange
            List<Player> players = new List<Player>
            {
                new Player(1, "Player1"),
                new Player(2, "Player2")
            };

            // Act
            FileManager.SaveToJson(players, TEST_FILE);

            // Assert
            Assert.IsTrue(File.Exists(TEST_FILE));
        }

        [TestMethod]
        public void LoadFromJson_NonExistentFile_ReturnsEmptyList()
        {
            // Arrange
            string nonExistentFile = "nonexistent.json";

            // Act
            List<Player> result = FileManager.LoadFromJson<Player>(nonExistentFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void SaveAndLoad_ValidData_RoundTripSucceeds()
        {
            // Arrange
            List<Player> originalPlayers = new List<Player>
            {
                new Player(1, "Player1"),
                new Player(2, "Player2")
            };

            // Act
            FileManager.SaveToJson(originalPlayers, TEST_FILE);
            List<Player> loadedPlayers = FileManager.LoadFromJson<Player>(TEST_FILE);

            // Assert
            Assert.AreEqual(originalPlayers.Count, loadedPlayers.Count);
            Assert.AreEqual(originalPlayers[0].PlayerID, loadedPlayers[0].PlayerID);
            Assert.AreEqual(originalPlayers[0].UserName, loadedPlayers[0].UserName);
        }

        [TestMethod]
        public void LoadFromJson_EmptyFile_ReturnsEmptyList()
        {
            // Arrange
            File.WriteAllText(TEST_FILE, "");

            // Act
            List<Player> result = FileManager.LoadFromJson<Player>(TEST_FILE);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void LoadFromJson_MalformedJson_ReturnsEmptyList()
        {
            // Arrange
            File.WriteAllText(TEST_FILE, "{ invalid json }");

            // Act
            List<Player> result = FileManager.LoadFromJson<Player>(TEST_FILE);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FileExists_ExistingFile_ReturnsTrue()
        {
            // Arrange
            File.WriteAllText(TEST_FILE, "test");

            // Act
            bool exists = FileManager.FileExists(TEST_FILE);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void FileExists_NonExistentFile_ReturnsFalse()
        {
            // Arrange
            string nonExistentFile = "nonexistent.json";

            // Act
            bool exists = FileManager.FileExists(nonExistentFile);

            // Assert
            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void SaveToJson_WithNestedGames_PreservesStructure()
        {
            // Arrange
            Player player = new Player(1, "TestPlayer");
            player.AddGame("Minecraft");
            player.AddHoursToGame("Minecraft", 10.5);
            player.UpdateGameHighScore("Minecraft", 100);

            List<Player> players = new List<Player> { player };

            // Act
            FileManager.SaveToJson(players, TEST_FILE);
            List<Player> loadedPlayers = FileManager.LoadFromJson<Player>(TEST_FILE);

            // Assert
            Assert.AreEqual(1, loadedPlayers.Count);
            Assert.AreEqual(1, loadedPlayers[0].Games.Count);
            Assert.AreEqual("Minecraft", loadedPlayers[0].Games[0].GameName);
            Assert.AreEqual(10.5, loadedPlayers[0].Games[0].HoursPlayed);
            Assert.AreEqual(100, loadedPlayers[0].Games[0].HighScore);
        }
    }
}
