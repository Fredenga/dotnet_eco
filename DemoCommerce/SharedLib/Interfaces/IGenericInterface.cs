using SharedLib.Responses;
using System.Linq.Expressions;

namespace SharedLib.Interfaces
{
    public interface IGenericInterface<T> where T: class
    {
        Task<Response> CreateAsync(T entity);
        Task<Response> UpdateAsync(T entity);
        Task<Response> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIDAsync(int id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
