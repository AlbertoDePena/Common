using System;

namespace Numaka.Common.Contracts
{
    /// <summary>
    /// SQL Server GUID Factory Interface
    /// </summary>
    public interface ISqlServerGuidFactory
    {
        /// <summary>
        /// SQL Server GUID
        /// </summary>
        /// <returns></returns>
        Guid NewSqlServerGuid();
    }

    /// <summary>
    /// Oracle GUID Factory Interface
    /// </summary>
    public interface IOracleGuidFactory
    {
        /// <summary>
        /// Oracle GUID
        /// </summary>
        /// <returns></returns>
        Guid NewOracleGuid();
    }

    /// <summary>
    /// MySql GUID Factory Interface
    /// </summary>
    public interface IMySqlGuidFactory
    {
        /// <summary>
        /// My SQL GUID
        /// </summary>
        /// <returns></returns>
        Guid NewMySqlGuid();
    }

    /// <summary>
    /// Postgres GUID Factory Interface
    /// </summary>
    public interface IPostgresGuidFactory
    {
        /// <summary>
        /// Postgres GUID
        /// </summary>
        /// <returns></returns>
        Guid NewPostgresGuid();
    }

    /// <summary>
    /// GUID Factory Interface
    /// </summary>
    public interface IGuidFactory : ISqlServerGuidFactory, IOracleGuidFactory, IMySqlGuidFactory, IPostgresGuidFactory { }
}