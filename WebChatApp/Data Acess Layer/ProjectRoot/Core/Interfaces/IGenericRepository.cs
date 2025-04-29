using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.ProjectRoot.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task AddAsync(T entity);

        Task<T> FindAsync(Expression<Func<T, bool>> predicate); 
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
                                                                        
        Task<T?> GetByIdGuideAsync(string id);
        Task AddRangeAsync(List<T> entity);
        void Update(T entity);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        void Delete(T entity);


    }
}
