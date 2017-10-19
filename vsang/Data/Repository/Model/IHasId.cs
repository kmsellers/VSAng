using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSAng.Data.Repository.Model
{
    public interface IHasId
    {
        Guid id { get; set; }

    }

}
