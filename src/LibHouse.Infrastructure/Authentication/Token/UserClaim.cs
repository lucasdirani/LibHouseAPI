namespace LibHouse.Infrastructure.Authentication.Token
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