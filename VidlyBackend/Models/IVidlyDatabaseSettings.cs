﻿namespace VidlyBackend.Models
{
    public interface IVidlyDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}