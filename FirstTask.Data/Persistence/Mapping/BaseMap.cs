using FirstTask.Data.Domain;
using System;
using System.Data.Entity.ModelConfiguration;

namespace FirstTask.Data.Persistence.Mapping
{
    public class BaseMap<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public BaseMap()
        {
            this.HasKey(e => e.Id);
        }
    }
}
