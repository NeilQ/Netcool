using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Netcool.Api.Core.EfCore;

namespace Netcool.Api.Core
{
    /**
     * 理论上Ef core的DbContext已经具备Uow + Repository模式的功能，但考虑到可能更换ORM的情况，
     * 这里仍然简单封装了一层，以防万一。
     * 实际上BeginTransaction抽象得不完美，仍然依赖了EF，我尝试再抽象一层ITransaction，
     * 但发现需要功能更丰富的注入框架，比如Autofac，目前还不打算引入。
     */
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
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
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