namespace LibHouse.API.V1.ViewModels.Users
{
    /// <summary>
    /// Representa uma claim pertencente ao usuário
    /// </summary>
    public class UserClaimViewModel
    {
        /// <summary>
        /// O valor associado à claim
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// O nome de identificação da claim
        /// </summary>
        public string Type { get; }

        public UserClaimViewModel(string value, string type)
        {
            Value = value;
            Type = type;
        }
    }
}