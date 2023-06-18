namespace CultureAPI.Infrastructure.Queries
{
    public class RefreshTokensQueries
    {
        public static string addRefreshToken = @"INSERT INTO RefreshTokens
            (UserId, JwtId, Token, isUsed, isRevoked, AddedDate, ExpiryDate) 
            VALUES (@UserId, @JwtId, @Token, @isUsed, @isRevoked, @AddedDate, @ExpiryDate)";

        public static string getRefreshToken = @"SELECT * FROM RefreshTokens WHERE Token = @Token";

        public static string updateTokenStatus = "UPDATE RefreshTokens SET IsUsed = @IsUsed WHERE Token = @Token";
    }
}
