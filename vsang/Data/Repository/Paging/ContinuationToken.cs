using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace VSAng.Data.Repository.Paging
{
    public class ContinuationToken
    {
        #region Constructor

        public ContinuationToken(string token)
        {
            this.Token = token;
        }

        #endregion Constructor

        #region Properties

        public string Token { get; private set; }
        public bool CanPage => !string.IsNullOrEmpty(this.Token);

        #endregion Properties
    }
}

