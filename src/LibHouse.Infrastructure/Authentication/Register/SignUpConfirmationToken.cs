namespace LibHouse.Infrastructure.Authentication.Register
{
    public class SignUpConfirmationToken
    {
        public string Value { get; }

        public SignUpConfirmationToken(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"Confirmation token: {Value}";
        }
    }
}