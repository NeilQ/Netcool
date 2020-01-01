using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Netcool.Api.Core
{
    public interface IUnitOfWork
    {
        TransactionScope BeginTransactionScope();

        IDbContextTransaction BeginTransaction();

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}