﻿namespace CultureAPI.Infrastructure.Services
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public TimeSpan ExpiryTimeFrame { get; set; }

    }
}
