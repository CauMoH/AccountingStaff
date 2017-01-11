using DataAccess.Interfaces;
using DataAccess.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class RepositoryInitializer
    {
        public static void Initialize(IUnityContainer container, Func<string> getAppDataFolderPath)
        {
            Database.SetInitializer(new DbInitializer());
            DbRepository.GetAppDataFolderPath = getAppDataFolderPath;

            container.RegisterType<IEmployeesRepository, EmployeesRepository>(new ContainerControlledLifetimeManager());
        }
    }
}
