namespace VidlyBackend.Profiles
{
    public class VidlyDatabaseSettings : IVidlyDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
