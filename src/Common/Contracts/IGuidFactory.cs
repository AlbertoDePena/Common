using System;

namespace Numaka.Common.Contracts
{
    /// <summary>
    /// GUID Factory Interface
    /// </summary>
    public interface IGuidFactory
    {
        /// <summary>
        /// SQL Server GUID
        /// </summary>
        /// <returns></returns>
        Guid NewSqlServerGuid();

        /// <summary>
        /// Oracle GUID
        /// </summary>
        /// <returns></returns>
        Guid NewOracleGuid();

        /// <summary>
        /// My SQL GUID
        /// </summary>
        /// <returns></returns>
        Guid NewMySqlGuid();

        /// <summary>
        /// Postgres GUID
        /// </summary>
        /// <returns></returns>
        Guid NewPostgresGuid();
    }
}