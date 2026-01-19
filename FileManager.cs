using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace CET2007
{
    /// <summary>
    /// FileManager implements Repository Pattern for data persistence
    /// Handles JSON serialization/deserialization with robust error handling
    /// </summary>
    public class FileManager
    {
        /// <summary>
        /// Save data to JSON file with error handling
        /// </summary>
        public static void SaveToJson<T>(List<T> data, string filename)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(filename, jsonString);
                Logger.GetInstance().Log($"Successfully saved {data.Count} items to {filename}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.GetInstance().Log($"ERROR: Access denied when saving to {filename}: {ex.Message}");
                throw new IOException($"Cannot save to {filename}. Access denied.", ex);
            }
            catch (IOException ex)
            {
                Logger.GetInstance().Log($"ERROR: IO error when saving to {filename}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Log($"ERROR: Unexpected error when saving to {filename}: {ex.Message}");
                throw new IOException($"Failed to save data to {filename}", ex);
            }
        }

        /// <summary>
        /// Load data from JSON file with comprehensive error handling
        /// </summary>
        public static List<T> LoadFromJson<T>(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Logger.GetInstance().Log($"File {filename} does not exist. Returning empty list.");
                    return new List<T>();
                }

                string jsonData = File.ReadAllText(filename);

                // Handle empty file
                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    Logger.GetInstance().Log($"File {filename} is empty. Returning empty list.");
                    return new List<T>();
                }

                List<T> data = JsonSerializer.Deserialize<List<T>>(jsonData);
                
                if (data == null)
                {
                    Logger.GetInstance().Log($"Deserialization returned null for {filename}. Returning empty list.");
                    return new List<T>();
                }

                Logger.GetInstance().Log($"Successfully loaded {data.Count} items from {filename}");
                return data;
            }
            catch (JsonException ex)
            {
                Logger.GetInstance().Log($"ERROR: Malformed JSON in {filename}: {ex.Message}");
                Console.WriteLine($"Warning: The file {filename} contains invalid JSON data. Starting with empty data.");
                return new List<T>();
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.GetInstance().Log($"ERROR: Access denied when reading {filename}: {ex.Message}");
                throw new IOException($"Cannot read {filename}. Access denied.", ex);
            }
            catch (IOException ex)
            {
                Logger.GetInstance().Log($"ERROR: IO error when reading {filename}: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Log($"ERROR: Unexpected error when loading {filename}: {ex.Message}");
                Console.WriteLine($"Warning: Failed to load data from {filename}. Starting with empty data.");
                return new List<T>();
            }
        }

        /// <summary>
        /// Check if a file exists
        /// </summary>
        public static bool FileExists(string filename)
        {
            return File.Exists(filename);
        }
    }
}
