

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MobileHome.Insure.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MobileHome.Insure.Web.App_Start.NinjectWebCommon), "Stop")]

namespace MobileHome.Insure.Web.App_Start
{
    using System;
    using System.Web;
    using System.Linq;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Modules;
using System.Collections.Generic;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                LoadDependencyModules(kernel);
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

        }

        private static void LoadDependencyModules(IKernel kernel)
        {
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetModules().Select(y=>y.Name.StartsWith("mobilehome.insure")));
//            foreach (var assembly in assemblies)
//                (Activator.CreateInstance(typeof(assembly)) as NinjectModule).Load();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.StartsWith("DAL") || x.FullName.StartsWith("Service") || x.FullName.StartsWith("mobilehome.insure"));
            //GetModules().Where(p => p.Name.StartsWith("DAL") || p.Name.StartsWith("Service") || p.Name.StartsWith("mobilehome.insure")));
            //foreach (var assembly in assemblies.GetModules().Select(x => x.GetTypes().Where(y => y.IsClass && !y.IsAbstract && y.IsAssignableFrom(typeof(NinjectModule))))
            //{
            //   var module = assembly.GetModules().Select(x => x.GetTypes().Where(y => y.IsClass && !y.IsAbstract && y.IsAssignableFrom(typeof(NinjectModule))));
            //    (Activator.CreateInstance(typeof(assembly)) as NinjectModule).Load();
            //}

        }

    }
}
