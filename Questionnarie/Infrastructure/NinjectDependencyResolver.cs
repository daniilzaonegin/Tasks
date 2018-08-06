using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Tasks.Abstract;
using Tasks.Helpers;
using Tasks.Model.Abstract;
using Tasks.Model.Concrete;
using Tasks.Models;

namespace Tasks.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            kernel.Bind<IUserTasksRepository>().To<EFUserTasksRepository>();
            kernel.Bind<IAppSettings>().To<AppSettings>();
            kernel.Bind<IEmailSender>().To<SmtpEmailSender>().WithConstructorArgument("settings",new AppSettings());
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType); 
        }
    }
}