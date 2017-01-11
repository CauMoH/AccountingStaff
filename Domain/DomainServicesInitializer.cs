using Domain.Interfaces;
using Domain.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class DomainServicesInitializer
    {
        public static void Initialize(IUnityContainer container)
        {
            container.RegisterType<IEmployeesModelService, EmployeesModelService>(new ContainerControlledLifetimeManager());
        }
    }
}
