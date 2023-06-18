namespace CultureAPI.Infrastructure.Queries
{
    public static class UsersInitiativesQueries
    {
        public static string AddSupportedInitiative = @"insert into UsersInitiatives (UserId, InitiativeId)
            values (@UserId, @InitiativeId)";

        public static string CheckSupportedInitiative = @"SELECT Count(*) as InitiativeCheckCode FROM  
            dbo.UsersInitiatives WHERE UserId = @UserId AND InitiativeId = @InitiativeId";
    }
}
