using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    internal sealed class AccountingStaffContext : DbContext
    {
        public AccountingStaffContext(string connectionString)
            : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }

        #region Tables

        public DbSet<EmployeeEntity> Employees { get; set; }

        #endregion

        #region Model configuration

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new EmployeeEntity.Configuration());

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Entity helpers

        public void AddOrUpdateEntity<TEntity>(TEntity entity)
            where TEntity : EntityBase
        {
            EnsureAttached(entity);          
            Entry(entity).State = (entity.Id == 0 ? EntityState.Added : EntityState.Modified);
        }

        public void DeleteEntity<TEntity>(TEntity entity)
            where TEntity : EntityBase
        {
            EnsureAttached(entity);
            Entry(entity).State = EntityState.Deleted;
        }

        public void UpdateProperty<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> selector)
            where TEntity : EntityBase
        {
            EnsureAttached(entity);
            Entry(entity).Property(selector).IsModified = true;
        }

        private void EnsureAttached<TEntity>(TEntity entity)
             where TEntity : EntityBase
        {
            if (!Set<TEntity>().Local.Contains(entity))
            {
                Set<TEntity>().Attach(entity);
            }
        }

        #endregion
    }
}
