using Dashboard.Data;
using Dashboard.DataAccess.Repo.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Repo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ProjectContext _projectContext;
        private readonly DbSet<T> _dbset;
        public Repository(ProjectContext projectContext)
        {
            _projectContext = projectContext;
            this._dbset = projectContext.Set<T>();  
            
        }
        public IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }
        public IQueryable<T> CustomeGetAll()
        {
            return _dbset;
           

        }
        public T Get(object id)
        {
            return _dbset.Find(id);
        }
        public T GetById(Expression<Func<T, bool>> preducate)
        {
            IQueryable<T> query = _dbset;
            query = query.Where(preducate);
            return query.FirstOrDefault();

        }
        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }
        public void Add(T entity)
        {
            _dbset.Add(entity);
        }
    }
}
