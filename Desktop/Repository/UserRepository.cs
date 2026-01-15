using Desktop;
using Microsoft.Data.Sqlite;
using System;

namespace Desktop
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public static class UserRepository
    {
        private static string connectionString = "Data Source=todo.db";

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Email TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL,
                    Name TEXT NOT NULL
                )";

                command.ExecuteNonQuery();
                TaskRepository.InitializeDatabase();
            }
        }

        public static User AuthenticateUser(string email, string password)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT Id, Name, Email FROM Users WHERE Email = @Email AND Password = @Password";
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2)
                        };
                    }
                }
            }

            return null;
        }

        public static bool RegisterUser(string email, string password, string name)
        {
            try
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO Users (Email, Password, Name) VALUES (@Email, @Password, @Name)";
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Name", name);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                return false;
            }
        }
    }
}