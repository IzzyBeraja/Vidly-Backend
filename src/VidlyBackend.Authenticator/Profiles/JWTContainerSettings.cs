namespace Authenticator.Profiles
{
    public class JWTContainerSettings : IAuthContainerSettings
    {
        public string SecretKey { get; set; }
        public string SecurityAlgorithm { get; set; }
        public int ExpireMinutes { get; set; }
    }
}
