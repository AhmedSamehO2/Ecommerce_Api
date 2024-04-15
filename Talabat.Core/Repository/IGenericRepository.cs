using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Core.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        Task<T> GetByIdwithSpecAsync(ISpecification<T> spec);

        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
    }
}
