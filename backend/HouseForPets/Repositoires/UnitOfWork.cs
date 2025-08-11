using DataBaseContext;
using Microsoft.EntityFrameworkCore.Storage;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBasePrimaryContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        public IPetRepository pets { get; }
        public IUserRepository users { get; }
        public IFavRepository favs { get; }
        public UnitOfWork(DataBasePrimaryContext context, IPetRepository petRepository, IUserRepository usersRepository, IFavRepository favsRepository)
        {
            _context = context;
            pets = petRepository;
            users = usersRepository;
            favs = favsRepository;
        }
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Транзакция уже начата.");
            }
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Транзакция не была начата.");
            }
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                try
                {
                    await _transaction.RollbackAsync();
                }
                finally
                {
                    await DisposeTransactionAsync();
                }
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DisposeTransactionAsync().Wait();
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
