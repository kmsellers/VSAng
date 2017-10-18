using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VSAng.Data.Repository;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;

namespace VSAng.Data
{
    internal static class DocumentDbConfigExtensions
    {
        internal static async Task CreateDatabaseIfNotExistsAsync(this DocumentDbConfig documentDbConfig, ILogger logger)
        {
            try
            {
                logger?.LogTrace("ReadDatabaseAsync {DatabaseId}", documentDbConfig.DatabaseId);
                await documentDbConfig.Client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(documentDbConfig.DatabaseId));
                return;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    logger?.LogError(new EventId(), e, "ReadDatabaseAsync {DatabaseId}", documentDbConfig.DatabaseId);
                    throw;
                }
            }

            using (logger?.BeginScope("Create Database {DatabaseId}", documentDbConfig.DatabaseId))
            {
                try
                {
                    logger?.LogInformation("Create DocumentDb ...");
                    await documentDbConfig.Client.CreateDatabaseAsync(new Database { Id = documentDbConfig.DatabaseId });
                }
                catch (Exception ex)
                {
                    logger?.LogError(new EventId(), ex, "Create DocumentDb Failed");
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        internal static async Task CreateCollectionIfNotExistsAsync(this DocumentDbConfig documentDbConfig, ILogger logger, string collectionId)
        {
            try
            {
                var uri = UriFactory.CreateDocumentCollectionUri(documentDbConfig.DatabaseId, collectionId);
                var resource = await documentDbConfig.Client.ReadDocumentCollectionAsync(uri);


                return;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    logger?.LogError(new EventId(), e, "CreateCollectionIfNotExistsAsync {CollectionId}", collectionId);
                    throw;
                }
            }

            using (logger?.BeginScope("Create Collection {CollectionId}", collectionId))
            {
                var newDocumentCollection = new DocumentCollection { Id = collectionId };

                logger?.LogInformation("Create Collection");

                var uri = UriFactory.CreateDatabaseUri(documentDbConfig.DatabaseId);
                var documentCollection = await documentDbConfig.Client.CreateDocumentCollectionAsync(uri, newDocumentCollection, new RequestOptions { OfferThroughput = 1000 });
            }
        }

    }
}
