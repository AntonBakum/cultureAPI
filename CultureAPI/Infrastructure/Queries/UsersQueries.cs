using System.Net.NetworkInformation;

namespace CultureAPI.Infrastructure.Queries
{
    public static class UsersQueries
    {
        public static string createUser = @"INSERT INTO Users (Name, Email, Password, PasswordSalt) OUTPUT INSERTED.Id
        Values (@Name, @Email, @Password, @PasswordSalt)";

        public static string getAllUsers = @"SELECT * FROM Users";

        public static string getUserByEmail = "SELECT * FROM Users WHERE Email = @Email";

        public static string getUserById = "SELECT Id, Email, Name, Phone, Nickname FROM Users WHERE Id = @Id";

        public static string updateUser = @"UPDATE Users SET @Name = Name, Email = @Email, Phone = @Phone, 
              Nickname = @Nickname WHERE Id = @Id";
    }
}
