namespace LibHouse.Infrastructure.Authentication.Token
{
    public class TokenSettings
    {
        public string Secret { get; set; }
        public int ExpiresInHours { get; set; }
        public string Issuer { get; set; }
        public string ValidIn { get; set; }
    }
}