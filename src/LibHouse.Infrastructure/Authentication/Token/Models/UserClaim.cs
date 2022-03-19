namespace LibHouse.Infrastructure.Authentication.Token.Models
{
    public class UserClaim
    {
        public string Value { get; }
        public string Type { get; }

        public UserClaim(string value, string type)
        {
            Value = value;
            Type = type;
        }
    }
}