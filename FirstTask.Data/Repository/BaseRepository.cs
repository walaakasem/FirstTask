using System;
using System.Linq;
using System.Data.Entity;
using FirstTask.Data.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using FirstTask.Data;

namespace FirstTask.Data.Repository
{
    public class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        public IQueryable<TEntity> All => DbSet.AsNoTracking();
        private FirstTaskContext Context { get; set; } /*ClothesContext.GetInstance();*/
        protected DbSet<TEntity> DbSet { get; }

        public BaseRepository(FirstTaskContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        #region Add

        public TEntity Add(TEntity entity)
        {
            //UpdateCreatedAndModifiedDates(entity);
            Context.Entry(entity).State = EntityState.Added;
            var _entity = DbSet.Add(entity);
            Save();
            return _entity;
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            //UpdateCreatedAndModifiedDates(entities);
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Added;
            }
            DbSet.AddRange(entities);
            Save();

        }

        #endregion
        #region Update
        public TEntity Update(TEntity entity)
        {
            bool isDetached = Context.Entry(entity).State == EntityState.Detached;
            if (isDetached)
            {
                entity = DbSet.Attach(entity);
            }

            Context.Entry(entity).State = EntityState.Modified;
            Save();

            Context.Entry(entity).State = EntityState.Detached;

            return entity;
        }
        public void UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Update(entity);
            }
        }
        public void Update(TEntity entity, string[] propsToUpdate)
        {
            bool isDetached = Context.Entry(entity).State == EntityState.Detached;
            if (isDetached)
            {
                DbSet.Attach(entity);
            }
            var entry = Context.Entry(entity);
            foreach (var prop in propsToUpdate)
            {
                var errors = entry.Property(prop).GetValidationErrors();
                if (!errors.Any())
                    entry.Property(prop).IsModified = true;
            }
            Save();
            Context.Entry(entity).State = EntityState.Detached;

        }

        public void Update(IList<TEntity> entities, string[] propsToUpdate)
        {
            foreach (var entity in entities)
            {
                DbSet.Attach(entity);
                var entry = Context.Entry(entity);
                foreach (var prop in propsToUpdate)
                {
                    var errors = entry.Property(prop).GetValidationErrors();
                    if (!errors.Any())
                        entry.Property(prop).IsModified = true;
                }
            }
            Save();
            foreach (var entity in entities)
                Context.Entry(entity).State = EntityState.Detached;

        }


        #endregion
        #region Delete
        public TEntity Delete(long id)
        {
            var entity = DbSet.Find(id);
            Context.Entry(entity).State = EntityState.Deleted;
            var _entity = DbSet.Remove(entity);
            Save();
            return _entity;
        }
        public async Task<TEntity> DeleteAsync(long id)
        {
            var entity = await DbSet.FindAsync(id);
            Context.Entry(entity).State = EntityState.Deleted;
            var _entity = DbSet.Remove(entity);
            Save();
            return _entity;
        }
        public void DeleteRange(IList<long> idList)
        {
            var entities = DbSet.Where(e => idList.Contains(e.Id));
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Deleted;
            }
            DbSet.RemoveRange(entities);

            Save();

        }

        #endregion
        #region Get
        public IList<TEntity> Get()
        {
            return All.AsNoTracking().OrderBy(e => e.Id).ToList();
        }
        public IQueryable<TEntity> AllInclude(string[] includes)
        {
            var query = DbSet.AsQueryable();
            foreach (var inc in includes)
            {
                query = query.Include(inc);
            }
            return query.AsNoTracking();
        }

        public TEntity Get(long id, string[] includes)
        {
            return AllInclude(includes).AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public TEntity Get(long id)
        {
            return DbSet.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> pred)
        {
            return DbSet.AsNoTracking().FirstOrDefault(pred);
        }

        #endregion
        public bool IsExist<TSource>(Expression<Func<TSource, bool>> filter) where TSource : BaseEntity
        {
            var lookupSet = Context.Set<TSource>();
            return lookupSet.AsNoTracking().Any(filter);
        }

        //public void SetUserId(long userId)
        //{
        //    this.UserId = userId;
        //}


        public void Save()
        {
            try
            {
                SetAuditProperties();
                Context.SaveChanges();

            }
            catch (DbUpdateException e)
            {

                Console.WriteLine(e);
                throw;
            }
        }

        public Task SaveAsync()
        {
            SetAuditProperties();
            return Context.SaveChangesAsync();
        }
        private void SetAuditProperties()
        {
            var entities = Context.ChangeTracker
                                   .Entries()
                                   .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added && x.Entity != null && typeof(BaseEntity).IsAssignableFrom(x.Entity.GetType()))
                                   .ToList();

            foreach (var entry in entities)
            {
                if (entry.Entity is BaseEntity)
                {
                    var entity = entry.Entity as BaseEntity;
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            {
                                entity.CreatedOn = DateTime.Now;
                                entity.ModifiedOn = DateTime.Now;
                                entity.CreatedBy = Common.UserId;
                                break;
                            }
                        case EntityState.Deleted:
                        case EntityState.Modified:
                            {
                                entity.ModifiedOn = DateTime.Now;
                                entity.ModifiedBy = Common.UserId;
                                break;
                            }
                    }
                }
            }
        }

    }
}
