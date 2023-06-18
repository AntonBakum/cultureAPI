namespace CultureAPI.Infrastructure.Queries
{
    public static class InitiativesQueries
    {
        public static string addInitiative = @"INSERT INTO Initiatives (Username, UserId, Title, CreationDate, NumberOfSupporters, Description)
              OUTPUT INSERTED.Id VALUES (@Username, @UserId,  @Title, @CreationDate, @NumberOfSupporters, @Description)";

        public static string getAllInitiatives = @"SELECT * FROM Initiatives";

        public static string getTopInitiatives = @"SELECT TOP 10 Id, Title, UserName, NumberOfSupporters, CreationDate FROM Initiatives
               ORDER BY NumberOfSupporters DESC;";

        public static string updateNumberOfSupporters = @"UPDATE Initiatives SET NumberOfSupporters = @NumberOfSupporters 
                WHERE Id = @Id";

        public static string getInitiativeForEmail = @"SELECT * FROM Initiatives WHERE Id = @Id";

        public static string updateInitiative = @"UPDATE Initiative SET Title = @Title, Description = @Description WHERE Id = @Id";

        public static string deleteInitiative = @"DELETE FROM Initiatives WHERE Id = @Id";

        public static string getInitiativeById = "SELECT Id, Title, CreationDate as Date, Description FROM Initiatives WHERE UserId = @Id";

    }
}
