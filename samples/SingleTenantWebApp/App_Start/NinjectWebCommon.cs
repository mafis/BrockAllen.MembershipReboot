[assembly: WebActivator.PreApplicationStartMethod(typeof(CIC.IdentityManager.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(CIC.IdentityManager.Web.App_Start.NinjectWebCommon), "Stop")]

namespace CIC.IdentityManager.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using BrockAllen.MembershipReboot;

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
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            var config = MembershipRebootConfig.Create();
            kernel.Bind<MembershipRebootConfiguration>().ToConstant(config);
            kernel.Bind<AuthenticationService>().To<SamAuthenticationService>();

            //RegisterEntityFramework(kernel);
            RegisterMongoDb(kernel);
            //RegisterRavenDb(kernel);
        }

        //private static void RegisterEntityFramework(IKernel kernel)
        //{
        //    System.Data.Entity.Database.SetInitializer<DefaultMembershipRebootDatabase>(new System.Data.Entity.CreateDatabaseIfNotExists<DefaultMembershipRebootDatabase>());
        //    kernel.Bind<IUserAccountRepository>().ToMethod(ctx => new DefaultUserAccountRepository()).InRequestScope();
        //}

        // To use MongoDB:
        // - Add a reference to the BrockAllen.MembershipReboot.MongoDb project.
        // - Uncomment this method.
        // - Call this method instead of RegisterEntityFramework in the RegisterServices method above.

        private static void RegisterMongoDb(IKernel kernel)
        {
            kernel.Bind<BrockAllen.MembershipReboot.MongoDb.MongoDatabase>().ToSelf().WithConstructorArgument("connectionStringName", "MongoDb");
            kernel.Bind<IUserAccountRepository>().To<BrockAllen.MembershipReboot.MongoDb.MongoUserAccountRepository>();
        }


        // To use RavenDB::
        // - Add a reference to the BrockAllen.MembershipReboot.RavenDb project.
        // - Uncomment this method.
        // - Call this method instead of RegisterEntityFramework in the RegisterServices method above.

        //private static void RegisterRavenDb(IKernel kernel)
        //{
        //    kernel.Bind<IUserAccountRepository>().ToMethod(ctx => new BrockAllen.MembershipReboot.RavenDb.RavenUserAccountRepository("RavenDb"));
        //}        
    }
}
