using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VSAng.Data.Repository;

namespace VSAng.Data
{
    public class DocumentDbBootstrapper
    {
        private static readonly ILogger _logger;

        static DocumentDbBootstrapper()
        {
            _logger = Log.CreateLogger<DocumentDbBootstrapper>();
        }

        public static async Task Initialize()
        {
            await CloudDbConfig.VSAngDbConfig.CreateDatabaseIfNotExistsAsync(_logger);
        }

        public static async Task CreateAllCollections()
        {
            Console.WriteLine("Creating missing collections ...");
            var modelAssembly = typeof(DocumentDbBootstrapper).GetTypeInfo().Assembly;

            var lstVSAngTypes =
                modelAssembly.DefinedTypes.Where(
                    item => item.GetCustomAttribute<CollectionAttribute>() != null &&
                            item.GetCustomAttribute<CollectionAttribute>().Database == CollectionDatabases.VSAng);

            foreach (var item in lstVSAngTypes)
            {
                Console.WriteLine("Creating collection '" + item.Name + "' in VSAng ...");

                await CloudDbConfig.VSAngDbConfig.CreateCollectionIfNotExistsAsync(_logger, item.Name);
            }

            var lstToDoTypes =
                modelAssembly.DefinedTypes.Where(
                    item => item.GetCustomAttribute<CollectionAttribute>() != null &&
                            item.GetCustomAttribute<CollectionAttribute>().Database == CollectionDatabases.ToDo);

            foreach (var item in lstToDoTypes)
            {
                Console.WriteLine("Creating collection '" + item.Name + "' in ToDo ...");

                await CloudDbConfig.ToDoDbConfig.CreateCollectionIfNotExistsAsync(_logger, item.Name);
            }
        }
    }
}
