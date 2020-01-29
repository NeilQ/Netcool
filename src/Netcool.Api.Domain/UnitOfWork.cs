using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Netcool.Api.Domain.EfCore;
using Netcool.Core.EfCore;

namespace Netcool.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        protected DbContext DbContext { get; }

        /// <summary>
        /// Creates a new <see cref="UnitOfWork"/>.
        /// </summary>
        public UnitOfWork(NetcoolDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TransactionScope BeginTransactionScope()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted});
        }

        public IDbContextTransaction BeginTransaction()
        {
            return DbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
    }
}