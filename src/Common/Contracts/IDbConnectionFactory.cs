using System.Data;

namespace Numaka.Common.Contracts
{
    /// <summary>
    /// Database connection factory
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Create a new database connection
        /// </summary>
        IDbConnection Create();
    }
}