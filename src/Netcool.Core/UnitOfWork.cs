﻿using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Netcool.Core.EfCore;

namespace Netcool.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        protected IDbContext DbContext { get; }

        /// <summary>
        /// Creates a new <see cref="UnitOfWork"/>.
        /// </summary>
        public UnitOfWork(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TransactionScope BeginTransactionScope()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return DbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
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
