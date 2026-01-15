using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Desktop
{
    public static class TaskRepository
    {
        private static string connectionString = "Data Source=todo.db";

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Category TEXT NOT NULL,
                    Description TEXT,
                    CreatedDate TEXT NOT NULL,
                    DueDate TEXT,
                    IsCompleted INTEGER NOT NULL DEFAULT 0,
                    CompletedDate TEXT,
                    UserId INTEGER NOT NULL
                )";

                command.ExecuteNonQuery();
                command = connection.CreateCommand();
                command.CommandText = @"
                PRAGMA table_info(Tasks)";

                bool hasCompletedDate = false;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(1) == "CompletedDate")
                        {
                            hasCompletedDate = true;
                            break;
                        }
                    }
                }

                if (!hasCompletedDate)
                {
                    command = connection.CreateCommand();
                    command.CommandText = @"
                    ALTER TABLE Tasks ADD COLUMN CompletedDate TEXT";
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Task> GetTasksByUser(int userId)
        {
            var tasks = new List<Task>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT Id, Title, Category, Description, CreatedDate, DueDate, 
                       IsCompleted, CompletedDate, UserId 
                FROM Tasks 
                WHERE UserId = @UserId 
                ORDER BY CreatedDate DESC";

                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new Task
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Category = reader.GetString(2),
                            Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            CreatedDate = DateTime.Parse(reader.GetString(4)),
                            UserId = reader.GetInt32(8),
                            IsCompleted = reader.GetInt32(6) == 1
                        };

                        if (!reader.IsDBNull(5))
                        {
                            task.DueDate = DateTime.Parse(reader.GetString(5));
                        }

                        if (!reader.IsDBNull(7))
                        {
                            task.CompletedDate = DateTime.Parse(reader.GetString(7));
                        }

                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }

        public static bool CreateTask(Task task)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO Tasks (Title, Category, Description, CreatedDate, DueDate, 
                                   IsCompleted, CompletedDate, UserId) 
                VALUES (@Title, @Category, @Description, @CreatedDate, @DueDate, 
                        @IsCompleted, @CompletedDate, @UserId)";

                command.Parameters.AddWithValue("@Title", task.Title);
                command.Parameters.AddWithValue("@Category", task.Category);
                command.Parameters.AddWithValue("@Description", task.Description ?? "");
                command.Parameters.AddWithValue("@CreatedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@DueDate", task.DueDate.HasValue ?
                    task.DueDate.Value.ToString("yyyy-MM-dd HH:mm:ss") : (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);
                command.Parameters.AddWithValue("@CompletedDate",
                    task.CompletedDate.HasValue ?
                    task.CompletedDate.Value.ToString("yyyy-MM-dd HH:mm:ss") :
                    (object)DBNull.Value);
                command.Parameters.AddWithValue("@UserId", task.UserId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public static bool UpdateTaskStatus(int taskId, bool isCompleted)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                UPDATE Tasks 
                SET IsCompleted = @IsCompleted, 
                    CompletedDate = CASE 
                        WHEN @IsCompleted = 1 THEN datetime('now') 
                        ELSE NULL 
                    END
                WHERE Id = @Id";

                command.Parameters.AddWithValue("@IsCompleted", isCompleted ? 1 : 0);
                command.Parameters.AddWithValue("@Id", taskId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public static bool DeleteTask(int taskId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Tasks WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", taskId);

                int result = command.ExecuteNonQuery();
                return result > 0;
            }
        }

        public static List<string> GetAllCategories(int userId)
        {
            var categories = new List<string>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT DISTINCT Category FROM Tasks WHERE UserId = @UserId";
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(reader.GetString(0));
                    }
                }
            }

            return categories;
        }
    }
}
