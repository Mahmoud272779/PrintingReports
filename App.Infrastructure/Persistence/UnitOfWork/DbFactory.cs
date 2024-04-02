using App.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Persistence.UnitOfWork
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private  Func<ClientSqlDbContext> _instanceFunc;
        private ClientSqlDbContext _dbContext;
        public ClientSqlDbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

        public DbFactory(Func<ClientSqlDbContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
               _dbContext.Dispose();
            }
        }
    }
}
