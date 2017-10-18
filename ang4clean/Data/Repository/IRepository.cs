using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Microsoft.Azure.Documents;
using VSAng.Data.Repository.Model;
using VSAng.Data.Repository.Paging;

namespace VSAng.Data.Repository
{
    public interface IRepository<T> where T : class, IHasId
    {
       // IRegion Region { get; }

        Task<T> AddDocument(T document, string overrideRegion = null);

        Task<PagedResult<T>> Paged(int itemsPerPage, string sqlQuery, Dictionary<string, object> parameters, string continuationToken = null, string overrideRegion = null, bool includeServerCount = false);
        Task<PagedResult<T>> Paged(int itemsPerPage, bool includeServerCount = false, Expression<Func<T, bool>> search = null);
        Task<PagedResult<T>> Paged(PagedResult<T> pagedResult);

        Task<T> GetDocument(Guid id, string overrideRegion = null);
        Task UpdateDocument(T document, string overrideRegion = null);
        Task RemoveDocument(Guid id, string overrideRegion = null);

        Task<IEnumerable<T>> GetDocuments(Expression<Func<T, bool>> predicate, int maxItemCount = -1, string overrideRegion = null);
        Task<T> GetFirstDocument(Expression<Func<T, bool>> predicate, string overrideRegion = null);
        Task<KeyValuePair<string, T>> GetFirstDocumentAllSources(Expression<Func<T, bool>> predicate);

        Task<bool> Exists(Guid id, string overrideRegion = null);
       // Task<string> CreateMediaAttachment(Guid id, Stream stream, string contentType, string slug);
       // Task<Attachment> GetAttachment(string documentId, string attachmentId);
       // Task<KeyValuePair<Attachment, Stream>> GetAttachmentStream(string documentId, string attachmentId);

        Task<IEnumerable<T>> GetDocumentsAllSources(Expression<Func<T, bool>> predicate, int maxItemCount = -1);
        Task<IEnumerable<Guid>> GetDocumentIdsAllSources(Expression<Func<T, bool>> predicate, int maxItemCount = -1);

        Task<IEnumerable<T>> GetDocumentsOrdered<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderby, int maxItemCount = -1, string overrideRegion = null);
        Task<IEnumerable<T>> GetDocumentsOrderedDesc<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderbyDesc, int maxItemCountPerSource = -1, string overrideRegion = null);
        Task<IEnumerable<T>> GetDocumentsAllSourcesOrdered<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderby, int maxItemCountPerSource = -1);

        Task<IEnumerable<T>> GetDocumentsAllSourcesOrderedDesc<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderbyDesc, int maxItemCountPerSource = -1);
        List<IQueryable<T>> GetDocumentsAllSourcesQuery(Expression<Func<T, bool>> predicate, int maxItemCountPerSource = -1);

        Task<IEnumerable<T>> GetDocumentsAllSources(string sqlQuery, Dictionary<string, object> parameters);

        Task<IEnumerable<T>> GetDocuments(string sqlQuery, Dictionary<string, object> parameters, string overrideRegion = null);
        Task<IEnumerable<dynamic>> GetDynamicDocuments(string sqlQuery, Dictionary<string, object> parameters, string overrideRegion = null);
        Task<IEnumerable<Guid>> GetDocumentIds(Expression<Func<T, bool>> predicate, string overrideRegion = null);

       // void OverrideRegion(IRegion overrideRegion);

        IQueryable<T> Query(string region = null);
    }
}