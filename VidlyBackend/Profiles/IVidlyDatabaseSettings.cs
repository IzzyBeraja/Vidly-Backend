namespace VidlyBackend.Profiles
{
    public interface IVidlyDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}