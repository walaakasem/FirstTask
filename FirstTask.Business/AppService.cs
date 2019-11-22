using FirstTask.Data.Domain;
using FirstTask.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FirstTask.Business
{
    public class AppService<T> where T : BaseEntity
    {
        //protected UnitOfWork _unitOfWork { get { return new UnitOfWork(); } }
        protected BaseRepository<T> _repository;
        protected long UserId { get; private set; }
        public AppService(BaseRepository<T> repository)
        {
            _repository = repository;
        }

        public T Add(T entity)
        {
            var updatedEnity = _repository.Add(entity);
            return updatedEnity;
        }

        public T Update(T entity)
        {
            T updatedEnity = _repository.Update(entity);
            return updatedEnity;
        }
        public void Update(T entity, string[] propsToUpdate)
        {
            _repository.Update(entity, propsToUpdate);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            T updatedEnity = _repository.Update(entity);
            return updatedEnity;
        }

        public T Delete(long id)
        {
            T updatedEnity = _repository.Delete(id);
            return updatedEnity;
        }

        public async Task<T> DeleteAsync(long id)
        {
            T updatedEnity = await _repository.DeleteAsync(id);
            return updatedEnity;
        }

        public T Get(long id)
        {
            return _repository.Get(id);
        }

        public IList<T> GetAll()
        {
            return _repository.Get();
        }
        public IList<T> AllInclude(string[] includes)
        {
            return _repository.AllInclude(includes).ToList();
        }
        public T Get(long id, string[] includes)
        {
            return _repository.Get(id, includes);
        }
    }
}
