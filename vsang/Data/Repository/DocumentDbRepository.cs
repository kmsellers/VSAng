using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using VSAng.Data.Repository.Model;
using VSAng.Data.Repository.Paging;

namespace VSAng.Data.Repository
{
    public class DocumentDbRepository<T> : IRepository<T>, IDisposable where T : class, IHasId
    {
        private readonly ILogger _logger = Log.CreateLogger<T>();

       // public IRegion Region { get; private set; }

        //public DocumentDbRepository(IRegion region)
        //{
        //    Region = region;
        //}

        //public void OverrideRegion(IRegion overrideRegion)
        //{
        //    Region = overrideRegion;
        //}

        public string CollectionId => typeof(T).Name;



        /// <summary>
        ///     Gets the document client
        /// </summary>        
        public DocumentClient GetDocumentClient()
        {
            return CloudDbConfig.VSAngDbConfig.Client;
        }

        /// <summary>
        ///     Gets the region code
        /// </summary>
        /// <param name="overrideRegion">Region code that will override the result</param>        
        public string GetRegion(string overrideRegion)
        {

            return overrideRegion ?? "na"; //Region.Code
        }


        /// <summary>
        ///     Gets the database identifier
        /// </summary>

        public string GetDatabaseId()
        {
            return CloudDbConfig.VSAngDbConfig.DatabaseId;
        }

        /// <summary>
        ///     Disposes the no-administrated resources
        /// </summary>
        public void Dispose()
        {

        }

        #region IRepository

        ///// <summary>
        /////     Add a document to the database
        ///// </summary>
        ///// <param name="document">Document that will be added</param>
        ///// <param name="overrideRegion">Region code</param>        
        //public async Task<bool> AddDocument(T document, string overrideRegion = null)
        //{
        //    if (document.id == Guid.Empty)
        //    {
        //        document.id = Guid.NewGuid();
        //    }

        //    using (_logger?.BeginScope("AddDocument {id}", document.id))
        //    {
        //        _logger?.LogInformation("Add Document [" + document.id + "] ");

        //        var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
        //        var doc = await GetDocumentClient().CreateDocumentAsync(uri, document);

        //        if (doc != null)
        //        {
        //            return doc.StatusCode == HttpStatusCode.Created;
        //        }

        //        _logger?.LogWarning("Did not create document  [" + document.id + "] {document}", document);
        //        return false;
        //    }
        //}
        public async Task<T> AddDocument(T document, string overrideRegion = null)
        {
            if (document.id == Guid.Empty)
            {
                document.id = Guid.NewGuid();
            }

            using (_logger?.BeginScope("AddDocument {id}", document.id))
            {
                _logger?.LogInformation("Add Document [" + document.id + "] ");

                var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
                var doc = await GetDocumentClient().CreateDocumentAsync(uri, document);

                if (doc != null && doc.StatusCode == HttpStatusCode.Created)
                {
                    return document;
                }

                _logger?.LogWarning("Did not create document  [" + document.id + "] {document}", document);
                return null;
            }
        }
        /// <summary>
        ///     Gets the paged result of a SQL query
        /// </summary>
        /// <param name="itemsPerPage">Items per page</param>
        /// <param name="sqlQuery">SQL query</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="continuationToken">Continuation token</param>
        /// <param name="overrideRegion">Region code</param>
        /// <param name="includeServerCount">True, if the result will include the server count, false otherwise</param>
        public async Task<PagedResult<T>> Paged(int itemsPerPage, string sqlQuery, Dictionary<string, object> parameters, string continuationToken = null, string overrideRegion = null, bool includeServerCount = false)
        {
            long serverCount = 0;

            if (itemsPerPage > 100)
            {
                itemsPerPage = 100;
            }

            var feedOptions = new FeedOptions
            {
                MaxItemCount = itemsPerPage
            };

            if (!string.IsNullOrEmpty(continuationToken))
            {
                feedOptions.RequestContinuation = continuationToken;
            }

            using (_logger?.BeginScope("Paged {sqlQuery} {parameters}", sqlQuery, parameters))
            {
                var col = new SqlParameterCollection();

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        col.Add(new SqlParameter(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value));
                    }
                }

                var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
                var query = GetDocumentClient().CreateDocumentQuery<T>(uri, new SqlQuerySpec(sqlQuery, col), feedOptions).AsQueryable();

                if (includeServerCount)
                {
                    serverCount = query.AsEnumerable().LongCount();
                }

                var feedResponse = await query.AsDocumentQuery().ExecuteNextAsync<T>();

                return new PagedResult<T>(serverCount, itemsPerPage, feedResponse.ToArray(), feedResponse.ResponseContinuation, GetRegion(overrideRegion));
            }
        }

        /// <summary>
        ///     Gets the paged result of documents that satisfy a boolean condition
        /// </summary>
        /// <param name="itemsPerPage">Items per page</param>
        /// <param name="includeServerCount">True, if the result will include the server count, false otherwise</param>
        /// <param name="search">Boolean condition that must satisfy the elements of the result</param>
        public async Task<PagedResult<T>> Paged(int itemsPerPage, bool includeServerCount = false, Expression<Func<T, bool>> search = null)
        {
            long serverCount = 0;

            if (itemsPerPage > 100)
            {
                itemsPerPage = 100;
            }

            var feedOptions = new FeedOptions
            {
                MaxItemCount = itemsPerPage
            };

            var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
            var query = GetDocumentClient().CreateDocumentQuery<T>(uri, feedOptions).AsQueryable();

            if (search != null)
            {
                query = query.Where(search);
            }

            // Opt in because it requires a extra service call
            if (includeServerCount)
            {
                serverCount = query.AsEnumerable().LongCount();
            }

            var feedResponse = await query.AsDocumentQuery().ExecuteNextAsync<T>();

            return new PagedResult<T>(serverCount, itemsPerPage, feedResponse.ToArray(), feedResponse.ResponseContinuation, GetRegion(null));
        }

        /// <summary>
        ///     Gets the next page of a paged result
        /// </summary>
        /// <param name="pagedResult">Paged result</param>        
        public async Task<PagedResult<T>> Paged(PagedResult<T> pagedResult)
        {
            if (!pagedResult.ContinuationToken.CanPage)
            {
                return null;
            }

            var feedOptions = new FeedOptions
            {
                MaxItemCount = pagedResult.ItemsPerPage,
                RequestContinuation = pagedResult.ContinuationToken.Token
            };

            var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
            var query = GetDocumentClient().CreateDocumentQuery<T>(uri, feedOptions).AsQueryable();
            var feedResponse = await query.AsDocumentQuery().ExecuteNextAsync<T>();

            return new PagedResult<T>(pagedResult.ServerCount, pagedResult.ItemsPerPage, feedResponse.ToArray(), feedResponse.ResponseContinuation, GetRegion(null));
        }

        /// <summary>
        ///     Get all the documents from the database
        /// </summary>
        /// <param name="maxItemCount">Maximum item count that will be returned</param>        
        public async Task<IEnumerable<dynamic>> GetOrigDocuments(int maxItemCount = -1)
        {
            var client = GetDocumentClient();

            if (client == null)
            {
                throw new KeyNotFoundException("Region not found");
            }

            using (_logger?.BeginScope("GetOrigDocuments"))
            {
                var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
                var query = client.CreateDocumentQuery<dynamic>(uri, new FeedOptions { MaxItemCount = maxItemCount }).AsDocumentQuery();

                var results = new List<dynamic>();

                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync());
                }

                _logger?.LogTrace("GetOrigDocuments {resultCount}", results.Count);

                return results;
            }
        }

        /// <summary>
        ///     Find the elements that satisfies a predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="maxItemCount">Maximum item count</param>
        /// <param name="overrideRegion">Region code</param>        
        public async Task<IEnumerable<T>> GetDocuments(Expression<Func<T, bool>> predicate, int maxItemCount = -1, string overrideRegion = null)
        {
            var client = GetDocumentClient();

            if (client == null)
            {
                throw new KeyNotFoundException($"Region {overrideRegion} not found");
            }

            using (_logger?.BeginScope("GetDocuments"))
            {
                IDocumentQuery<T> query;

                var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
                var feedOptions = new FeedOptions { MaxItemCount = maxItemCount, EnableCrossPartitionQuery = false };

                _logger?.LogTrace("GetDocuments {predicate}", predicate);

                if (predicate != null)
                {
                    query = client.CreateDocumentQuery<T>(uri, feedOptions).Where(predicate).AsDocumentQuery();
                }
                else
                {
                    query = client.CreateDocumentQuery<T>(uri, feedOptions).AsDocumentQuery();
                }

                var results = new List<T>();

                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<T>());
                }

                _logger?.LogTrace("GetDocuments {resultCount}", results.Count);

                return results;
            }
        }

        /// <summary>
        ///     Gets the typed result of executing a parameterized query
        /// </summary>
        /// <param name="sqlQuery">Sql query</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="overrideRegion">Region code</param>        
        public async Task<IEnumerable<T>> GetDocuments(string sqlQuery, Dictionary<string, object> parameters, string overrideRegion = null)
        {
            var results = new List<T>();

            using (_logger?.BeginScope("QueryDocuments {sqlQuery} {parameters}", sqlQuery, parameters))
            {
                var col = new SqlParameterCollection();

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        col.Add(new SqlParameter(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value));
                    }
                }

                var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
                var query = GetDocumentClient().CreateDocumentQuery<T>(uri, new SqlQuerySpec(sqlQuery, col)).AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<T>());
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets the dynamic result of executing a parameterized query
        /// </summary>
        /// <param name="sqlQuery">Sql query</param>
        /// <param name="parameters">Query parameters</param>
        /// <param name="overrideRegion">Region code</param>        
        public async Task<IEnumerable<dynamic>> GetDynamicDocuments(string sqlQuery, Dictionary<string, object> parameters, string overrideRegion = null)
        {
            var results = new List<dynamic>();

            using (_logger?.BeginScope("QueryDynamicDocuments {sqlQuery} {parameters}", sqlQuery, parameters))
            {
                var col = new SqlParameterCollection();

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        col.Add(new SqlParameter(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value));
                    }
                }

                var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
                var query = GetDocumentClient().CreateDocumentQuery<dynamic>(uri, new SqlQuerySpec(sqlQuery, col)).AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync());
                }
            }

            return results;
        }

        /// <summary>
        ///     Get the identifier of the documents that satisfies a predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="overrideRegion">Region code</param>        
        public async Task<IEnumerable<Guid>> GetDocumentIds(Expression<Func<T, bool>> predicate, string overrideRegion = null)
        {
            using (_logger?.BeginScope("GetDocuments"))
            {
                IDocumentQuery<Guid> query;

                var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
                var feedOptions = new FeedOptions { EnableCrossPartitionQuery = false };

                _logger?.LogTrace("GetDocuments {predicate}", predicate);

                if (predicate != null)
                {
                    query = GetDocumentClient()
                                .CreateDocumentQuery<T>(uri, feedOptions)
                                .Where(predicate)
                                .Select(doc => doc.id)
                                .AsDocumentQuery();
                }
                else
                {
                    query = GetDocumentClient()
                                .CreateDocumentQuery<T>(uri, feedOptions)
                                .Select(doc => doc.id)
                                .AsDocumentQuery();
                }

                var results = new List<Guid>();

                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<Guid>());
                }

                _logger?.LogTrace("GetDocuments {resultCount}", results.Count);

                return results;
            }
        }

        /// <summary>
        ///     Get the first document that satisfies a predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="overrideRegion">Region code</param>        
        public async Task<T> GetFirstDocument(Expression<Func<T, bool>> predicate, string overrideRegion = null)
        {
            using (_logger?.BeginScope("GetFirstDocument {predicate}", predicate))
            {
                var result = default(T);
                var lstDocuments = await GetDocuments(predicate, 1, overrideRegion);

                if (lstDocuments != null)
                {
                    result = lstDocuments.FirstOrDefault();

                    if (result == null)
                    {
                        _logger?.LogInformation("Not Found");
                    }
                }
                else
                {
                    _logger?.LogInformation("Not Found");
                }

                return result;
            }
        }

        /// <summary>
        ///     Get the first document from all the sources that satisfies a predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>        
        public async Task<KeyValuePair<string, T>> GetFirstDocumentAllSources(Expression<Func<T, bool>> predicate)
        {
            var endPointMap = new List<string>();

            using (_logger?.BeginScope("GetFirstDocumentAllSources {predicate}", predicate))
            {
                foreach (var db in GetDocumentDbConfig())
                {
                    if (endPointMap.Contains(db.Value.EndpointKey))
                    {
                        continue;
                    }

                    endPointMap.Add(db.Value.EndpointKey);

                    var uri = UriFactory.CreateDocumentCollectionUri(db.Value.DatabaseId, CollectionId);
                    var feedOptions = new FeedOptions { MaxItemCount = 1, EnableCrossPartitionQuery = false };

                    var query = db.Value.Client.CreateDocumentQuery<T>(uri, feedOptions).Where(predicate).AsDocumentQuery();

                    var results = new List<T>();

                    while (query.HasMoreResults)
                    {
                        results.AddRange(await query.ExecuteNextAsync<T>());
                    }

                    if (results.Count > 0)
                    {
                        _logger?.LogTrace("Found {Key}", db.Key);
                        return new KeyValuePair<string, T>(db.Key, results[0]);
                    }
                }

                _logger?.LogTrace("Not Found");
                return new KeyValuePair<string, T>("", null);
            }
        }

        /// <summary>
        ///     Get all the documents from all the sources that satisfies a predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>        
        public async Task<IEnumerable<T>> GetDocumentsAllSources(Expression<Func<T, bool>> predicate, int maxItemCountPerSource = -1)
        {
            var endPointMap = new List<string>();
            var results = new List<T>();

            using (_logger?.BeginScope("GetDocumentsAllSources {predicate}", predicate))
            {
                foreach (var db in GetDocumentDbConfig())
                {
                    if (endPointMap.Contains(db.Value.EndpointKey))
                    {
                        continue;
                    }

                    endPointMap.Add(db.Value.EndpointKey);

                    _logger?.LogTrace("GetDocumentsAllSources {Key}", db.Key);

                    var uri = UriFactory.CreateDocumentCollectionUri(db.Value.DatabaseId, CollectionId);
                    var feedOptions = new FeedOptions { MaxItemCount = maxItemCountPerSource, EnableCrossPartitionQuery = false };

                    var query = db.Value.Client.CreateDocumentQuery<T>(uri, feedOptions)
                                               .Where(predicate)
                                               .AsDocumentQuery();

                    while (query.HasMoreResults)
                    {
                        results.AddRange(await query.ExecuteNextAsync<T>());
                    }
                }
            }
            return results;
        }

        /// <summary>
        ///     Get all the document identifiers from all the sources that satisfies a predicate
        /// </summary>
        /// <param name="predicate">Predicate</param>        
        public async Task<IEnumerable<Guid>> GetDocumentIdsAllSources(Expression<Func<T, bool>> predicate, int maxItemCountPerSource = -1)
        {
            var endPointMap = new List<string>();
            var results = new List<Guid>();

            using (_logger?.BeginScope("GetDocumentIdsAllSources {predicate}", predicate))
            {
                foreach (var db in GetDocumentDbConfig())
                {
                    if (endPointMap.Contains(db.Value.EndpointKey))
                    {
                        continue;
                    }

                    endPointMap.Add(db.Value.EndpointKey);

                    _logger?.LogTrace("GetDocumentIdsAllSources {Key}", db.Key);

                    var uri = UriFactory.CreateDocumentCollectionUri(db.Value.DatabaseId, CollectionId);
                    var feedOptions = new FeedOptions { MaxItemCount = maxItemCountPerSource, EnableCrossPartitionQuery = false };

                    var query = db.Value.Client.CreateDocumentQuery<T>(uri, feedOptions)
                                               .Where(predicate)
                                               .Select(doc => doc.id)
                                               .AsDocumentQuery();

                    while (query.HasMoreResults)
                    {
                        results.AddRange(await query.ExecuteNextAsync<Guid>());
                    }
                }
            }
            return results;
        }

        /// <summary>
        ///     Gets the documents from all sources by a SQL query
        /// </summary>
        /// <param name="sqlQuery">SQL query</param>
        /// <param name="parameters">Parameters</param>        
        public async Task<IEnumerable<T>> GetDocumentsAllSources(string sqlQuery, Dictionary<string, object> parameters)
        {
            var endPointMap = new List<string>();
            var results = new List<T>();

            using (_logger?.BeginScope("GetDocumentsAllSources {sqlQuery} {parameters}", sqlQuery, parameters))
            {
                var sqlParameterCollection = new SqlParameterCollection();

                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        sqlParameterCollection.Add(new SqlParameter(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value));
                    }
                }

                foreach (var db in GetDocumentDbConfig())
                {
                    if (endPointMap.Contains(db.Value.EndpointKey))
                    {
                        continue;
                    }

                    endPointMap.Add(db.Value.EndpointKey);

                    var uri = UriFactory.CreateDocumentCollectionUri(db.Value.DatabaseId, CollectionId);
                    var sqlQuerySpec = new SqlQuerySpec(sqlQuery, sqlParameterCollection);

                    var query = db.Value.Client.CreateDocumentQuery<T>(uri, sqlQuerySpec).AsDocumentQuery();

                    while (query.HasMoreResults)
                    {
                        results.AddRange(await query.ExecuteNextAsync<T>());
                    }
                }
            }

            return results;
        }

        /// <summary>
        ///     Get all the documents ordered that satisfies a predicate
        /// </summary>
        /// <typeparam name="TKey">Data type of the field which will be used for ordering</typeparam>
        /// <param name="predicate">Predicate</param>
        /// <param name="orderby">OrderBy expression</param>
        /// <param name="maxItemCount">Maximum amount of items returned</param>
        /// <param name="overrideRegion">Region code</param>        
        public async Task<IEnumerable<T>> GetDocumentsOrdered<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderby, int maxItemCount = -1, string overrideRegion = null)
        {
            using (_logger?.BeginScope("GetDocumentsOrdered"))
            {
                IDocumentQuery<T> query;

                var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
                var feedOptions = new FeedOptions { MaxItemCount = maxItemCount, EnableCrossPartitionQuery = false };

                _logger?.LogTrace("GetDocumentsOrdered {predicate} {orderby}", predicate, orderby);

                if (predicate != null)
                {

                    query = GetDocumentClient()
                                .CreateDocumentQuery<T>(uri, feedOptions)
                                .Where(predicate)
                                .OrderBy(orderby)
                                .AsDocumentQuery();
                }
                else
                {
                    query = GetDocumentClient()
                                .CreateDocumentQuery<T>(uri, feedOptions)
                                .OrderBy(orderby)
                                .AsDocumentQuery();
                }

                var results = new List<T>();

                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<T>());
                }

                _logger?.LogTrace("GetDocumentsOrdered {resultCount}", results.Count);

                return results;
            }
        }

        /// <summary>
        ///     Gets the documents that satisfies a predicate ordered descending
        /// </summary>
        /// <typeparam name="TKey">Data type of the property used for ordering the information</typeparam>
        /// <param name="predicate">Predicate</param>
        /// <param name="orderbyDesc">OrderBy expression</param>
        /// <param name="maxItemCountPerSource">Maximum amount of items returned</param>        
        /// <param name="overrideRegion"></param>
        public async Task<IEnumerable<T>> GetDocumentsOrderedDesc<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderbyDesc, int maxItemCountPerSource = -1, string overrideRegion = null)
        {
            var results = new List<T>();
            var client = GetDocumentClient();

            _logger?.LogTrace("GetDocumentsOrderedDesc {predicate}", predicate);

            var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);
            var feedOptions = new FeedOptions { MaxItemCount = maxItemCountPerSource, EnableCrossPartitionQuery = false };

            var query = client.CreateDocumentQuery<T>(uri, feedOptions)
                              .Where(predicate)
                              .OrderByDescending(orderbyDesc)
                              .AsDocumentQuery();

            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        /// <summary>
        ///     Get the documents from all the sources ordered 
        /// </summary>
        /// <typeparam name="TKey">Data type of the property used for ordering the information</typeparam>
        /// <param name="predicate">Predicate</param>
        /// <param name="orderby">OrderBy expression</param>
        /// <param name="maxItemCountPerSource">Maximum amount of items returned</param>        
        public async Task<IEnumerable<T>> GetDocumentsAllSourcesOrdered<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderby, int maxItemCountPerSource = -1)
        {
            var endPointMap = new List<string>();
            var results = new List<T>();

            using (_logger?.BeginScope("GetDocumentsAllSources {predicate}", predicate))
            {
                foreach (var db in GetDocumentDbConfig())
                {
                    if (endPointMap.Contains(db.Value.EndpointKey))
                    {
                        continue;
                    }

                    endPointMap.Add(db.Value.EndpointKey);

                    _logger?.LogTrace("GetDocumentsAllSources {Key}", db.Key);

                    var feedOptions = new FeedOptions { MaxItemCount = maxItemCountPerSource, EnableCrossPartitionQuery = false };
                    var uri = UriFactory.CreateDocumentCollectionUri(db.Value.DatabaseId, CollectionId);

                    var query = db.Value.Client.CreateDocumentQuery<T>(uri, feedOptions)
                                               .Where(predicate)
                                               .OrderBy(orderby)
                                               .AsDocumentQuery();

                    while (query.HasMoreResults)
                    {
                        results.AddRange(await query.ExecuteNextAsync<T>());
                    }
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets the documents from all the sources that satisfies a predicate ordered descending
        /// </summary>
        /// <typeparam name="TKey">Data type of the property used for ordering the information</typeparam>
        /// <param name="predicate">Predicate</param>
        /// <param name="orderbyDesc">OrderBy expression</param>
        /// <param name="maxItemCountPerSource">Maximum amount of items returned</param>        
        public async Task<IEnumerable<T>> GetDocumentsAllSourcesOrderedDesc<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderbyDesc, int maxItemCountPerSource = -1)
        {
            var endPointMap = new List<string>();
            var results = new List<T>();

            using (_logger?.BeginScope("GetDocumentsAllSources {predicate}", predicate))
            {
                foreach (var db in GetDocumentDbConfig())
                {
                    if (endPointMap.Contains(db.Value.EndpointKey))
                    {
                        continue;
                    }

                    endPointMap.Add(db.Value.EndpointKey);

                    _logger?.LogTrace("GetDocumentsAllSources {Key}", db.Key);

                    var uri = UriFactory.CreateDocumentCollectionUri(db.Value.DatabaseId, CollectionId);
                    var feedOptions = new FeedOptions { MaxItemCount = maxItemCountPerSource, EnableCrossPartitionQuery = false };

                    var query = db.Value.Client.CreateDocumentQuery<T>(uri, feedOptions)
                                               .Where(predicate)
                                               .OrderByDescending(orderbyDesc)
                                               .AsDocumentQuery();

                    while (query.HasMoreResults)
                    {
                        results.AddRange(await query.ExecuteNextAsync<T>());
                    }
                }
            }

            return results;
        }

        /// <summary>
        ///     Gets the documents from all the sources that satisfies a predicate 
        /// </summary>
        /// <param name="predicate">Predicate</param>
        /// <param name="maxItemCountPerSource">Maximum amount of items returned</param>        
        public List<IQueryable<T>> GetDocumentsAllSourcesQuery(Expression<Func<T, bool>> predicate, int maxItemCountPerSource = -1)
        {
            var res = new List<IQueryable<T>>();
            var endPointMap = new List<string>();

            using (_logger?.BeginScope("GetDocumentsAllSources {predicate}", predicate))
            {
                foreach (var db in GetDocumentDbConfig())
                {
                    if (endPointMap.Contains(db.Value.EndpointKey))
                        continue;

                    endPointMap.Add(db.Value.EndpointKey);

                    _logger?.LogTrace("GetDocumentsAllSources {Key}", db.Key);

                    var uri = UriFactory.CreateDocumentCollectionUri(db.Value.DatabaseId, CollectionId);
                    var feedOptions = new FeedOptions { MaxItemCount = maxItemCountPerSource, EnableCrossPartitionQuery = false };

                    IQueryable<T> query = db.Value.Client.CreateDocumentQuery<T>(uri, feedOptions).Where(predicate);

                    res.Add(query);
                }
            }

            return res;
        }

        /// <summary>
        ///     Gets a document by the identifier
        /// </summary>
        /// <param name="id">Document identifier</param>
        /// <param name="overrideRegion">Region code</param>        
        public async Task<T> GetDocument(Guid id, string overrideRegion = null)
        {
            using (_logger?.BeginScope("GetDocument {id}", id))
            {
                _logger?.LogTrace($"Get document {nameof(T)} {id}");

                var lstDocuments = await GetDocuments(x => x.id == id, overrideRegion: overrideRegion);

                return lstDocuments?.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Returns true, if exists a document with the specified identifier, false otherwise
        /// </summary>
        /// <param name="id">Document identifier</param>
        /// <param name="overrideRegion">Region code</param>        
        public async Task<bool> Exists(Guid id, string overrideRegion = null)
        {
            using (_logger?.BeginScope("Exists {id}", id))
            {
                var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);

                var docExists = GetDocumentClient()
                                 .CreateDocumentQuery<T>(uri)
                                 .Where(doc => doc.id == id)
                                 .Select(doc => doc.id)
                                 .AsDocumentQuery();

                if (docExists.HasMoreResults)
                {
                    var key = await docExists.ExecuteNextAsync();
                    _logger?.LogTrace("True");

                    return key.Any();
                }

                _logger?.LogTrace("False");

                return false;
            }
        }

        /// <summary>
        ///     Removes a document by the identifier
        /// </summary>
        /// <param name="id">Document identifier</param>
        /// <param name="overrideRegion">Region code</param>        
        public async Task RemoveDocument(Guid id, string overrideRegion = null)
        {
            _logger?.LogInformation("RemoveDocument [" + id + "] {id}", id);

            var uri = UriFactory.CreateDocumentUri(GetDatabaseId(), CollectionId, id.ToString());

            await GetDocumentClient().DeleteDocumentAsync(uri);
        }

        /// <summary>
        ///     Updates a document
        /// </summary>
        /// <param name="document">Document</param>
        /// <param name="overrideRegion">Region code</param>
        public async Task UpdateDocument(T document, string overrideRegion = null)
        {
            _logger?.LogTrace("UpdateDocument [" + document.id + "] {id}", document.id);

            var uri = UriFactory.CreateDocumentUri(GetDatabaseId(), CollectionId, document.id.ToString());

            await GetDocumentClient().ReplaceDocumentAsync(uri, document);
        }

        #endregion

        #region Attachment

        ///// <summary>
        /////     Creates a text attachment associated to a document
        ///// </summary>
        ///// <param name="documentId">Document identifier</param>
        ///// <param name="attachmentId">Attachment identifier</param>
        ///// <param name="text">Message</param>        
        //public async Task<string> CreateTextAttachment(Guid documentId, string attachmentId, string text)
        //{
        //    using (_logger?.BeginScope("CreateTextAttachment {documentId} {attachmentID}"))
        //    {
        //        if (string.IsNullOrWhiteSpace(text) || documentId == Guid.Empty)
        //        {
        //            return null;
        //        }

        //        var docLink = UriFactory.CreateDocumentUri(GetDatabaseId(), CollectionId, documentId.ToString());

        //        var attachment = new Attachment
        //        {
        //            Id = attachmentId,
        //            ContentType = "text/plain",
        //            MediaLink = text
        //        };

        //        Attachment newAttachment = await GetDocumentClient().CreateAttachmentAsync(docLink, attachment);

        //        _logger?.LogTrace("Created Attachment {id}", newAttachment.Id);

        //        return newAttachment.Id;
        //    }
        //}

        ///// <summary>
        /////     Updates a text attachment 
        ///// </summary>
        ///// <param name="documentId">Document identifier</param>
        ///// <param name="attachmentId">Attachment identifier</param>
        ///// <param name="text">Message</param>        
        //public async Task<bool> UpdateTextAttachment(Guid documentId, string attachmentId, string text)
        //{
        //    if (string.IsNullOrWhiteSpace(text) || documentId == Guid.Empty)
        //    {
        //        return false;
        //    }

        //    var attachment = await GetAttachment(documentId.ToString(), attachmentId);

        //    if (attachment == null)
        //    {
        //        return false;
        //    }

        //    attachment.MediaLink = text;

        //    _logger?.LogTrace("UpdateTextAttachment {documentId} {attachmentID}");

        //    await GetDocumentClient().ReplaceAttachmentAsync(attachment);

        //    return true;
        //}

        /// <summary>
        ///     Creates a file attachment associated to a document
        /// </summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="fileStream">File stream</param>
        /// <param name="contentType">Content type</param>        
        /// <param name="slugName">Slug name</param>        
        public async Task<string> CreateMediaAttachment(Guid documentId, Stream fileStream, string contentType, string slugName)
        {
            using (_logger?.BeginScope("CreateMediaAttachment {documentId}", documentId))
            {
                if (fileStream == null || fileStream.Length == 0 || string.IsNullOrWhiteSpace(slugName) || documentId == Guid.Empty)
                {
                    _logger?.LogWarning("Invalid Parameters {Length} {slugName} {documentId}", fileStream?.Length, slugName, documentId);
                    return null;
                }

                var documentLink = UriFactory.CreateDocumentUri(GetDatabaseId(), CollectionId, documentId.ToString());
                var attachment = (Attachment)null;

                try
                {
                    _logger?.LogTrace("CreateAttachment");

                    var mediaOptions = new MediaOptions
                    {
                        ContentType = contentType,
                        Slug = slugName
                    };

                    attachment = await GetDocumentClient().CreateAttachmentAsync(documentLink, fileStream, mediaOptions);
                }
                catch (DocumentClientException e)
                {
                    _logger?.LogError(new EventId(100, "Error"), e, "CreateAttachment");

                    if (e.StatusCode != HttpStatusCode.Conflict && e.StatusCode != HttpStatusCode.InternalServerError)
                    {
                        throw;
                    }

                    var attLink = UriFactory.CreateAttachmentUri(GetDatabaseId(), CollectionId, documentId.ToString(), slugName);

                    attachment = await GetDocumentClient().ReadAttachmentAsync(attLink);
                }

                if (attachment == null)
                {
                    _logger?.LogWarning("CreateAttachment failed");
                    return null;
                }

                _logger?.LogInformation("CreateAttachment {id}", attachment.Id);

                return attachment.Id;
            }
        }

        ///// <summary>
        /////     Updates a media attachment 
        ///// </summary>
        ///// <param name="documentId">Document identifier</param>
        ///// <param name="fileStream">File stream</param>
        ///// <param name="contentType">Content type</param>
        ///// <param name="slugName">Slug name</param>
        //public async Task<bool> UpdateMediaAttachment(Guid documentId, Stream fileStream, string contentType, string slugName)
        //{
        //    using (_logger?.BeginScope("UpdateMediaAttachment {documentId}", documentId))
        //    {
        //        if (fileStream == null || fileStream.Length == 0 || string.IsNullOrWhiteSpace(slugName) || documentId == Guid.Empty)
        //        {
        //            _logger?.LogWarning("Invalid Parameters {Length} {slugName} {documentId}", fileStream?.Length, slugName, documentId);
        //            return false;
        //        }

        //        var attachment = await GetAttachment(documentId.ToString(), slugName);

        //        if (attachment != null)
        //        {
        //            _logger?.LogInformation("Remove attachment {AltLink}", attachment.AltLink);
        //            await GetDocumentClient().DeleteAttachmentAsync(attachment.AltLink);
        //        }

        //        await CreateMediaAttachment(documentId, fileStream, contentType, slugName);

        //        return true;
        //    }
        //}

        ///// <summary>
        /////     Gets the attachments associated to a document
        ///// </summary>
        ///// <param name="documentId">Document identifier</param>        
        //public async Task<IEnumerable<IAttachmentInfo>> GetAttachments(Guid documentId)
        //{
        //    _logger?.LogTrace("GetAttachments {documentId}", documentId);

        //    var docLink = UriFactory.CreateDocumentUri(GetDatabaseId(), CollectionId, documentId.ToString());
        //    var uri = docLink.OriginalString + "/attachments/";
        //    var feedResponse = await GetDocumentClient().ReadAttachmentFeedAsync(uri);

        //    var attachmentInfo = new List<IAttachmentInfo>();

        //    foreach (var attachemnt in feedResponse)
        //    {
        //        var dbAttachmentInfo = new DocumentDbAttachmentInfo
        //        {
        //            Id = attachemnt.Id,
        //            AltLink = attachemnt.AltLink,
        //            ContentType = attachemnt.ContentType,
        //            MediaLink = attachemnt.MediaLink
        //        };

        //        attachmentInfo.Add(dbAttachmentInfo);
        //    }

        //    return attachmentInfo;
        //}

        ///// <summary>
        /////     Gets the attachment stream associated to a document
        ///// </summary>
        ///// <param name="documentId">Document identifier</param>
        ///// <param name="attachmentId">Attachment identifier</param>        
        //public async Task<KeyValuePair<Attachment, Stream>> GetAttachmentStream(string documentId, string attachmentId)
        //{
        //    _logger?.LogTrace("GetAttachmentStream {documentId} {attachmentId}", documentId, attachmentId);

        //    var attachment = await GetAttachment(documentId, attachmentId);

        //    if (attachment == null)
        //    {
        //        return new KeyValuePair<Attachment, Stream>(null, null);
        //    }

        //    var media = await GetDocumentClient().ReadMediaAsync(attachment.MediaLink);

        //    return new KeyValuePair<Attachment, Stream>(attachment, media.Media);
        //}

        ///// <summary>
        /////     Gets the attachment associated to a document
        ///// </summary>
        ///// <param name="documentId">Document identifier</param>
        ///// <param name="attachmentId">Attachment identifier</param>        
        //public async Task<Attachment> GetAttachment(string documentId, string attachmentId)
        //{
        //    try
        //    {
        //        _logger?.LogTrace("GetAttachment {documentId} {attachmentId}", documentId, attachmentId);
        //        var uri = UriFactory.CreateAttachmentUri(GetDatabaseId(), CollectionId, documentId, attachmentId);

        //        return await GetDocumentClient().ReadAttachmentAsync(uri);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(new EventId(), ex, "GetAttachment {documentId} {attachmentId}", documentId, attachmentId);
        //    }

        //    return null;
        //}

        ///// <summary>
        /////     Gets the media attachment by the media link
        ///// </summary>
        ///// <param name="mediaLink">Media link</param>        
        //public async Task<MediaResponse> GetMediaAttachment(string mediaLink)
        //{
        //    _logger?.LogTrace("GetMediaAttachment {mediaLink}", mediaLink);
        //    var content = await GetDocumentClient().ReadMediaAsync(mediaLink);

        //    return content;
        //}

        ///// <summary>
        /////     Deletes the attachment associated to a document
        ///// </summary>
        ///// <param name="documentId">Document identifier</param>
        ///// <param name="attachmentId">Attachment identifier</param>        
        //public async Task<bool> DeleteAttachment(string documentId, string attachmentId)
        //{
        //    _logger?.LogInformation("DeleteAttachment {documentId} {attachmentId}", documentId, attachmentId);

        //    var uri = UriFactory.CreateAttachmentUri(GetDatabaseId(), CollectionId, documentId, attachmentId);
        //    var attachment = await GetDocumentClient().DeleteAttachmentAsync(uri);

        //    return attachment != null;
        //}

        #endregion

        public IQueryable<T> Query(string region = null)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(GetDatabaseId(), CollectionId);

            return GetDocumentClient().CreateDocumentQuery<T>(uri);
        }

        private Dictionary<string, DocumentDbConfig> GetDocumentDbConfig()
        {
                return new Dictionary<string, DocumentDbConfig>
                {
                    { "dev", CloudDbConfig.VSAngDbConfig }
                };

            
        }

    }
}
