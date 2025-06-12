using System.Security.Cryptography;
using System.Text;

namespace NewsSite.BL
{
    public class User
    {
        private int userID;
        private string username;
        private string email;
        private string passwordHash;
        private bool isAdmin;
        private bool isLocked;

        private static List<User> UsersList = new List<User>();

        // Constructors
        public User() { }

        public User(User user)
        {
            this.UserID = user.UserID;
            this.Username = user.Username;
            this.Email = user.Email;
            this.PasswordHash = user.PasswordHash;
            this.IsAdmin = user.IsAdmin;
            this.IsLocked = user.IsLocked;
        }

        public User(int userID, string username, string email, string passwordHash, bool isAdmin, bool isLocked)
        {
            this.UserID = userID;
            this.Username = username;
            this.Email = email;
            this.PasswordHash = passwordHash;
            this.IsAdmin = isAdmin;
            this.IsLocked = isLocked;
        }

        // Properties
        public int UserID
        {
            get => userID;
            set => userID = value;
        }

        public string Username
        {
            get => username;
            set => username = value;
        }

        public string Email
        {
            get => email;
            set => email = value;
        }

        public string PasswordHash
        {
            get => passwordHash;
            set => passwordHash = value;
        }

        public bool IsAdmin
        {
            get => isAdmin;
            set => isAdmin = value;
        }

        public bool IsLocked
        {
            get => isLocked;
            set => isLocked = value;
        }

        // Static Methods

        public static bool ValidateUser(User user)
        {
            if (user == null) return false;

            try
            {
                user.UserID = ValidationHelper.ValidatePositive<int>(user.UserID, nameof(user.UserID));
                user.Username = ValidationHelper.ValidateString(user.Username, nameof(user.Username));
                user.Email = ValidationHelper.ValidateString(user.Email, nameof(user.Email));
                user.PasswordHash = ValidationHelper.ValidateString(user.PasswordHash, nameof(user.PasswordHash));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Register(User user)
        {
            if (!ValidateUser(user))
            {
                throw new Exception("Validation failed");
            }

            if (UsersList.Any(u => u.UserID == user.UserID || u.Email == user.Email))
            {
                throw new Exception("User already exists");
            }

            return true;
        }

        public bool Insert()
        {
            if (UsersList.Any(u => u.UserID == UserID))
            {
                return false;
            }
            UsersList.Add(this);
            return true;
        }

        public static List<User> Read()
        {
            return UsersList;
        }

        public static string HashPassword(string password)
        {
            using (var sha512 = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha512.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public static int NewID()
        {
            return UsersList.Count + 1;
        }
    }
}
