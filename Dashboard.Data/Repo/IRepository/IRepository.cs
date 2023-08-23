using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Repo.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IQueryable<T> CustomeGetAll();
        T GetById(Expression<Func<T, bool>> preducate);
        T Get(object id);

        void Delete (T entity);
        void Add(T entity);
    }
}
