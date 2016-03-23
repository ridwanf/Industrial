using Core;
using Industrial.Data.Domain;
using Industrial.Data.Repositories;
using Industrial.Repository.Repositories;
using Industrial.Service.Services;
using MahApps.Metro.Controls;
using Microsoft.Practices.Unity;

namespace Industrial.Wpf.Infrastructures
{
    public static class ContainerHelper
    {
        private static readonly IUnityContainer _container;

        static ContainerHelper()
        {
            _container = new UnityContainer();
           _container.RegisterType<IItemProductService, ItemProductService>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IItemProductRepository, ItemProductRepository>(
                new ContainerControlledLifetimeManager());
            _container.RegisterType<IUnitOfWork, EFUnitOfWork>(
               new ContainerControlledLifetimeManager());
            _container.RegisterType<IUnitOfWorkFactory, EFUnitOfWorkFactory>(
               new ContainerControlledLifetimeManager());
            _container.RegisterType<MainDataContext, MainDataContext>(
             new ContainerControlledLifetimeManager());
         
        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }
    }
}
