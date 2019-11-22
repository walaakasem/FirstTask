using FirstTask.Data.Persistence.Mapping;
using System;
using System.Data.Entity;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using DbContext = System.Data.Entity.DbContext;
using FirstTask.Data.Domain;

namespace FirstTask.Data
{
    public class FirstTaskContext : DbContext
    {
        public FirstTaskContext() : base("FirstTaskDb") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<FirstTaskContext>(null);
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
          
            modelBuilder.Configurations.Add(new UserMap());
          

            base.OnModelCreating(modelBuilder);
        }
    }
}
