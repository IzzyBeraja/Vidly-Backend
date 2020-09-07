namespace Authenticator.Profiles
{
    public interface IAuthContainerSettings
    {
        string SecretKey { get; set; }
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }
    }
}