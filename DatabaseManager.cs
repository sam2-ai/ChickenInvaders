using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace ChickenInvaders
{
    public class PlayerScore
    {
        public int Rank { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public DateTime PlayedAt { get; set; }
    }

    public static class DatabaseManager
    {
        private static string DatabasePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChickenInvaders.db");
        private static string ConnectionString => $"Data Source={DatabasePath};Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(DatabasePath))
            {
                SQLiteConnection.CreateFile(DatabasePath);
            }

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS PlayerScores (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL,
                        Score INTEGER NOT NULL,
                        PlayedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";
                
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void SaveScore(string username, int score)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO PlayerScores (Username, Score, PlayedAt) VALUES (@username, @score, @playedAt)";
                
                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@score", score);
                    command.Parameters.AddWithValue("@playedAt", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<PlayerScore> GetTopScores(int count = 10)
        {
            var scores = new List<PlayerScore>();
            
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string selectQuery = @"
                    SELECT Username, Score, PlayedAt 
                    FROM PlayerScores 
                    ORDER BY Score DESC 
                    LIMIT @count";
                
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@count", count);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        int rank = 1;
                        while (reader.Read())
                        {
                            scores.Add(new PlayerScore
                            {
                                Rank = rank++,
                                Username = reader.GetString(0),
                                Score = reader.GetInt32(1),
                                PlayedAt = reader.GetDateTime(2)
                            });
                        }
                    }
                }
            }
            
            return scores;
        }

        public static int GetPlayerRank(int score)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string countQuery = "SELECT COUNT(*) FROM PlayerScores WHERE Score > @score";
                
                using (var command = new SQLiteCommand(countQuery, connection))
                {
                    command.Parameters.AddWithValue("@score", score);
                    int higherScores = Convert.ToInt32(command.ExecuteScalar());
                    return higherScores + 1;
                }
            }
        }
    }
}
