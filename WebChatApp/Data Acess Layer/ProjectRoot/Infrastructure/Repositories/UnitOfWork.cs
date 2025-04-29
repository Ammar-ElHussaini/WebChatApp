using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebChatApp.Models;

namespace Data_Access_Layer.ProjectRoot.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly WebchatAppContext _context;
        private readonly Dictionary<Type, object> _repositories;
        public UnitOfWork(WebchatAppContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }
        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new GenericRepository<T>(_context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
