using DataAccess.Entities;
using DataAccess.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal abstract class DbRepository
    {
        private const string DbFolderName = "Employees";
        private const string DbFileName = "Employees.sdf";

        private readonly List<string> _initializedDbPaths = new List<string>();

        internal static Func<string> GetAppDataFolderPath { get; set; }

        private static string DbFilePath => Path.Combine(DbFolderPath, DbFileName);

        private static string DbFolderPath => Path.Combine(GetAppDataFolderPath(), DbFolderName);

        internal static string ConnectionString
        {
            get { return string.Format("Data Source={0}", DbFilePath); }
        }

        private void InitializeDb()
        {
            if (!Directory.Exists(DbFolderPath))
            {
                Directory.CreateDirectory(DbFolderPath);
            }

            using (var context = new AccountingStaffContext(ConnectionString))
            {
                var created = context.Database.CreateIfNotExists();
                context.Database.Initialize(false);
            }
        }

        private AccountingStaffContext GetContext()
        {
            if (!_initializedDbPaths.Contains(DbFilePath))
            {
                InitializeDb();
                _initializedDbPaths.Add(DbFilePath);
            }

            return new AccountingStaffContext(ConnectionString);
        }

        protected void SaveEntity<TEntity>(TEntity entity)
            where TEntity : EntityBase
        {
            SaveEntities(new[] { entity });
        }

        protected void SaveEntities<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : EntityBase
        {
            SaveWrapped(context =>
            {
                foreach (var entity in entities)
                {
                    context.AddOrUpdateEntity(entity);
                }
            });
        }

        protected void SaveWrapped(Action<AccountingStaffContext> onSave)
        {
            using (var context = GetContext())
            {
                try
                {
                    onSave(context);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var messages = ex.EntityValidationErrors
                        .SelectMany(e => e.ValidationErrors.Select(err => err.ErrorMessage));

                    var messagesStr = string.Join(Environment.NewLine, messages);

                    throw new DataSaveException($"Database validation errors occurred: {messagesStr}");
                }
                catch (Exception ex)
                {
                    throw new DataSaveException($"Database save error occurred.", ex);
                }
            }
        }

        protected TResult LoadWrapped<TResult>(Func<AccountingStaffContext, TResult> onLoad)
        {
            using (var context = GetContext())
            {
                try
                {
                    return onLoad(context);
                }
                catch (Exception ex)
                {
                    throw new DataLoadException($"Database load error occurred.", ex);
                }
            }
        }
    }
}
