using System;
using System.Web.Mvc;
using System.Web.Routing;
using CostsCalculator.Models.Abstract;
using CostsCalculator.Models.Concrete;
using Ninject;

namespace CostsCalculator.Infrastructure
{
    public class NinjectControllerFactory:DefaultControllerFactory
    {
        private IKernel ninjectkernel;

        public NinjectControllerFactory()
        {
             ninjectkernel = new StandardKernel();
             AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext,Type controllerType)
        {
            return controllerType == null ? null : (IController) ninjectkernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectkernel.Bind<IPurchasesRepositiry>().To<PurchasesRepository>();            
        }
    }
}