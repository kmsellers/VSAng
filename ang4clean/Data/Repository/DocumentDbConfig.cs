using System;
using Microsoft.Azure.Documents.Client;

namespace VSAng.Data.Repository
{
    public class DocumentDbConfig
    {
        public string DatabaseId { get; set; }
        public string Key { get; set; }
        public string Endpoint { get; set; }


        /// <summary>
        ///     Endpoint key
        /// </summary>
        public string EndpointKey => $"{DatabaseId}{Endpoint}{Key}";
        /// <summary>
        ///     Document client
        /// </summary>
        public DocumentClient Client { get; }

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="databaseId">Database identifier</param>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="key">Key</param>
        public DocumentDbConfig(string databaseId, string endpoint, string key)
        {
            var connectionPolicy = new ConnectionPolicy
            {
                EnableEndpointDiscovery = false
            };

            Client = new DocumentClient(new Uri(endpoint), key, connectionPolicy);
            DatabaseId = databaseId;
            Endpoint = endpoint;
            Key = key;
        }
    }
}

