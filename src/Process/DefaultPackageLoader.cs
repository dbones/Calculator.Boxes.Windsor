namespace Process
{
    using Boxes.Integration;
    using Boxes.Integration.Setup;
    using Boxes.Integration.Setup.Filters;
    using System.Collections.Generic;
    using System.Linq;
    using Boxes;
    using Boxes.Integration.Process;
    using Boxes.Integration.Factories;
    using Boxes.Integration.Trust;


    public class DefaultPackageLoader<TBuilder, TContainer> : ILoadProcess<TBuilder, TContainer>
    {
        private readonly PackageRegistry _packageRegistry;
        private readonly IIocFactory<TBuilder, TContainer> _ioCFactory;
        private readonly IProcessOrder _processOrder;
        private readonly IDefaultContainerSetup<TBuilder> _containerSetup;
        private readonly ITrustManager _trustManager;

        private readonly PipelineExecutorWrapper<RegistrationContext<TBuilder>> _iocPipeline = new PipelineExecutorWrapper<RegistrationContext<TBuilder>>();

        public DefaultPackageLoader(
            PackageRegistry packageRegistry,
            IIocFactory<TBuilder, TContainer> ioCFactory,
            IProcessOrder processOrder,
            IDefaultContainerSetup<TBuilder> containerSetup,
            ITrustManager trustManager)
        {
            _packageRegistry = packageRegistry;
            _ioCFactory = ioCFactory;
            _processOrder = processOrder;
            _containerSetup = containerSetup;
            _trustManager = trustManager;
        }

        public void LoadPackages(Application application, IEnumerable<string> packagesToEnable)
        {
            application.Container.TryDispose();
            var builder = _ioCFactory.CreateBuilder();

            //TODO: check if there are any missing packages, which also need to be enabled

            var loadablePackages =
                _packageRegistry.Packages
                    .Where(x => x.Loaded);
                    //.Where(x => packagesToEnable.Contains(x.Name));

            //get process Order
            IEnumerable<Package> packages = _processOrder.Arrange(loadablePackages);

            //find the types in each package (filter as much as we can)
            var processContexts =
                packages.Select(
                    x =>
                    {
                        ITypeRegistrationFilter typesFilter = _containerSetup.GetTypeRegistrationFilter(x.Name) ??
                                                              _containerSetup.DefaultTypeRegistrationFilter;

                        var context = new ProcessPackageContext(x, typesFilter.FilterTypes(x).ToArray());

                        //TODO: check to see if the context is trusted

                        return context;
                    }).ToList(); //save the result, as we may need multiple iterations

            //we need to register all the types with the tenants IoC first
            IEnumerable<RegistrationContext<TBuilder>> registrationContexts =
                processContexts
                .SelectMany(x => x.DependencyTypes)
                .Select(x => new RegistrationContext<TBuilder>(x, builder));

            _iocPipeline.UpdateTasksAsRequired(_containerSetup.Registrations);
            _iocPipeline.Execute(registrationContexts).Force();


            //create the container from the builder (if required)
            var container = _ioCFactory.CreateContainer(builder);
            application.Container = container;
        }
    }
}