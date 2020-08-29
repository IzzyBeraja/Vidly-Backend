namespace VidlyBackend.Profiles
{
    public class VidlyDatabaseSettings : IVidlyDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
