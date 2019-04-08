using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using MyFirstSite.Domain.Abstract;
using MyFirstSite.Domain.Concrete;
using MyFirstSite.Domain.Entities;
using Ninject;

namespace MyFirstSite.WebUI.Infrastructure{

    public class NinjectDependencyResolver : IDependencyResolver{

        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam) {
            kernel = kernelParam;
            AddBinding();
        }

        private void AddBinding() {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m=>m.Products).Returns(new List<Product> {
                new Product{Name="Football", Price=25},
                new Product{Name="Surf board",Price=17.9M},
                new Product{Name="Running shoes", Price=95}
            });
            //kernel.Bind<IProductRepository>().ToConstant(mock.Object); 
            kernel.Bind<IProductRepository>().To<EFProductRepository>(); 

            EmailSetting emailSetting=new EmailSetting {                                                               //1.create new instance HERE
                WriteAsFile=bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings",emailSetting);  //2.in controller invoke with parameter

        }

        //implementation IDependencyResolver

        //get type of controller and try to create instance of controller
        public object GetService(Type serviceType) { 
            return kernel.TryGet(serviceType);
        }

        //get multiple binders for one type                     
        public IEnumerable<object> GetServices(Type serviceType) {
            return kernel.GetAll(serviceType);
        }
    } 
}