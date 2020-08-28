using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidlyBackend.Models
{
    public class VidlyDatabaseSettings : IVidlyDatabaseSettings
    {
        public string MovieCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
