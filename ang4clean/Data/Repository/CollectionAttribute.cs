using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSAng.Data.Repository
{

    /// <summary>
    ///     CollectionDatabases.
    ///     Collection databases.
    /// </summary>
    public enum CollectionDatabases
    {
        /// <summary>
        ///     Visual Studio Angular Demo Database
        /// </summary>
        VSAng,

        /// <summary>
        /// Some other supported database 
        /// </summary>
        ToDo
    }

    /// <summary>
    ///     Collection.
    ///     This attribute is used to mark entities which does belong to the collection database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionAttribute : Attribute
    {
        /// <summary>
        ///     Database where is located the collection
        /// </summary>
        public CollectionDatabases Database { get; set; }

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="database">Database where is located the collection</param>
        public CollectionAttribute(CollectionDatabases database)
        {
            this.Database = database;
        }
    }
}
