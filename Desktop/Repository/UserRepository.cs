    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Repository
{
    internal class UserRepository
    {
        private static List<User> _users = new List<User>();

        public static bool RegisterUser(string login, string password, string name)
        {
            if (IsLoginTaken(login))
            {
                return false; // Логин занят
            }

            string hashedPassword = PasswordHasher.HashPassword(password);
            _users.Add(new User { Login = login, Password = hashedPassword, Name = name });
            return true;
        }

        public static bool AuthenticateUser(string login, string password)
        {
            foreach (var user in _users)
            {
                if (user.Login == login && PasswordHasher.VerifyPassword(password, user.Password))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsLoginTaken(string login)
        {
            foreach (var user in _users)
            {
                if (user.Login == login)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }

    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
