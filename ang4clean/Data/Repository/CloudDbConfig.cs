using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSAng.Data.Repository
{
    public static class CloudDbConfig
    {
       public static DocumentDbConfig VSAngDbConfig { get; set; }
       public static DocumentDbConfig ToDoDbConfig { get; set; }

        static CloudDbConfig()
        {
            var localDb = "https://localhost:8082/";

            VSAngDbConfig = new DocumentDbConfig(
                Environment.GetEnvironmentVariable("DOCUMENTDB_ID") ?? "VSAng",
                Environment.GetEnvironmentVariable("DOCUMENTDB_URL") ?? localDb,
                Environment.GetEnvironmentVariable("DOCUMENTDB_KEY") ?? "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            ToDoDbConfig = new DocumentDbConfig(
                Environment.GetEnvironmentVariable("DOCUMENTDB_ID") ?? "ToDo",
                Environment.GetEnvironmentVariable("DOCUMENTDB_URL") ?? localDb,
                Environment.GetEnvironmentVariable("DOCUMENTDB_KEY") ?? "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        }
    }
}
