using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSAng.Data.Repository.Paging
{
    public class PagedResult<T>
    {
        #region Constructor

        public PagedResult(long serverCount, int itemsPerPage, T[] items, string continuationToken, string region)
        {
            this.Items = items;
            this.ContinuationToken = new ContinuationToken(token: continuationToken);
            this.ServerCount = serverCount;
            this.ItemsPerPage = itemsPerPage;
            this.Region = region;
        }

        #endregion Constructor

        #region Properties

        public ContinuationToken ContinuationToken { get; private set; }
        public T[] Items { get; private set; }
        public long ServerCount { get; private set; }
        public int ItemsPerPage { get; private set; }
        public long ExpectedPages
        {
            get
            {
                if (ServerCount <= 0)
                {
                    return 0;
                }

                return (ServerCount + ItemsPerPage - 1) / ItemsPerPage;
            }
        }

        public System.Collections.Generic.Dictionary<int, double> ExecutionTimes { get; set; } = new System.Collections.Generic.Dictionary<int, double>();

        public string Region { get; private set; }

        #endregion Properties
    }
}
