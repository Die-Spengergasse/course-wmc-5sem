using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace TodoBackend.Models
{
    public class User : Entity<int>
    {
        public User(string name, string salt, string passHash)
        {
            Name = name;
            Salt = salt;
            PassHash = passHash;
        }

        public string Name { get; set; }
        [MaxLength(44)]
        public string Salt { get; set; }
        [MaxLength(44)]
        public string PassHash { get; set; }
        public bool CheckPassword(string password)
        {
            byte[] saltBytes = Convert.FromBase64String(Salt);
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            using var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 100000, HashAlgorithmName.SHA256);
            var passwordHash = Convert.ToBase64String(pbkdf2.GetBytes(32));
            return PassHash == passwordHash;

        }
        public static User GenerateUserWithPassword(string username, string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[32];
            rng.GetBytes(saltBytes);
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            using var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 100000, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            return new User(username, Convert.ToBase64String(saltBytes), Convert.ToBase64String(hashBytes));
        }
    }
}
