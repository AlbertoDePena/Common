using Numaka.Common.Exceptions;
using Numaka.Common.Contracts;
using System;
using System.Data;

namespace Numaka.Common
{
    /// <summary>
    /// Unit of work base class
    /// </summary>
    public abstract class UnitOfWorkBase : IDisposable
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private bool _disposed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbConnectionFactory"></param>
        /// <param name="guidFactory"></param>
        protected UnitOfWorkBase(IDbConnectionFactory dbConnectionFactory, IGuidFactory guidFactory)
        {
            _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
            GuidFactory = guidFactory ?? throw new ArgumentNullException(nameof(guidFactory));
        }

        /// <summary>
        ///     Preferred factory for generating new unique identifiers for entities.
        /// </summary>
        /// <remarks>Prefer this over using Guid.NewGuid()</remarks>
        protected IGuidFactory GuidFactory { get; }

        /// <summary>
        ///     Commits all changes made by consumers of the DbTransaction within a single operation.
        /// </summary>
        /// <exception cref="RepositoryException">Thrown when there is an error when trying to commit the database transaction</exception>
        public void Commit()
        {
            try
            {
                _dbTransaction?.Commit();
            }
            catch (Exception e)
            {
                _dbTransaction?.Rollback();

                throw new RepositoryException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _dbTransaction?.Dispose();
                _dbConnection?.Dispose();
            }

            _disposed = true;
        }

        /// <summary>
        ///     Creates and opens the database connection, or returns the already open connection if it has previously been initialized
        /// </summary>
        protected IDbConnection GetDbConnection()
        {
            if (_dbConnection == null)
            {
                _dbConnection = _dbConnectionFactory.Create();

                _dbConnection.Open();
            }

            return _dbConnection;
        }

        /// <summary>
        ///     Begins and returns new database transaction, or returns the already open transaction if it has previously been initialized
        /// </summary>
        protected IDbTransaction GetDbTransaction()
        {
            return _dbTransaction ?? (_dbTransaction = GetDbConnection().BeginTransaction());
        }
    }
}
