namespace LibHouse.Infrastructure.Authentication.Token.Models
{
    /// <summary>
    /// Representa uma claim pertencente ao usuário
    /// </summary>
    public class UserClaim
    {
        /// <summary>
        /// O valor associado à claim
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// O nome de identificação da claim
        /// </summary>
        public string Type { get; }

        public UserClaim(string value, string type)
        {
            Value = value;
            Type = type;
        }
    }
}