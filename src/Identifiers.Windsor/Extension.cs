using Boxes.Integration.Setup;
using Boxes.Integration.Setup.Registrations;
using Boxes.Windsor;
using Castle.Windsor;

namespace Identifiers.Windsor
{
    using Boxes.Integration.Extensions;
    using Castle.MicroKernel.Lifestyle;

    public class Extension : ISetupBoxesExtension<IDefaultContainerSetup<IWindsorContainer>>
    {
        public bool CanHandle(IDefaultContainerSetup<IWindsorContainer> extension)
        {
            return true;
        }

        public void Configure(IDefaultContainerSetup<IWindsorContainer> containerSetup)
        {
            containerSetup.AddRegistration(new Register()
                .Where(x => typeof(ISingletonDependency).IsAssignableFrom(x))
                .LifeStyle(typeof(SingletonLifestyleManager))
                .AssociateWith(Contracts.SelfAndAllInterfaces));

            containerSetup.AddRegistration(new Register()
                .Where(x => typeof(ITransientDependency).IsAssignableFrom(x))
                .LifeStyle(typeof(TransientLifestyleManager))
                .AssociateWith(Contracts.SelfAndAllInterfaces));

            containerSetup.AddRegistration(new Register()
                .Where(x => x.HasAttribute<SingletonDependencyAttribute>())
                .LifeStyle(typeof(SingletonLifestyleManager))
                .AssociateWith(Contracts.SelfAndAllInterfaces));

            containerSetup.AddRegistration(new Register()
                .Where(x => x.HasAttribute<TransientDependencyAttribute>())
                .LifeStyle(typeof(TransientLifestyleManager))
                .AssociateWith(Contracts.SelfAndAllInterfaces));

            containerSetup.AddPackgeLevelFilter(new Filter(), "Identifiers");
        }
    }
}