namespace Identifiers.Windsor
{
    using Boxes.Integration;
    using Boxes.Integration.ContainerSetup;
    using Boxes.Integration.Extensions;
    using Castle.MicroKernel.Lifestyle;

    public class Extension : IBoxesExtension
    {
        public void Extend(IBoxesWrapper boxes)
        {
            var singleton = new Registration()
                .Where(x => typeof(ISingletonDependency).IsAssignableFrom(x))
                .LifeStyle<SingletonLifestyleManager>()
                .RegisterWith(RegisterWith.SelfAndAllInterfaces);

            var tranient = new Registration()
                .Where(x => typeof(ITransientDependency).IsAssignableFrom(x))
                .LifeStyle<TransientLifestyleManager>()
                .RegisterWith(RegisterWith.SelfAndAllInterfaces);

            var singletonAttr = new Registration()
                .Where(x => x.HasAttribute<SingletonDependencyAttribute>())
                .LifeStyle<SingletonLifestyleManager>()
                .RegisterWith(RegisterWith.SelfAndAllInterfaces);

            var tranientAttr = new Registration()
                .Where(x => x.HasAttribute<TransientDependencyAttribute>())
                .LifeStyle<TransientLifestyleManager>()
                .RegisterWith(RegisterWith.SelfAndAllInterfaces);


            boxes.BoxesContainerSetup.RegisterLifeStyle(tranient);
            boxes.BoxesContainerSetup.RegisterLifeStyle(singleton);

            boxes.BoxesContainerSetup.RegisterLifeStyle(tranientAttr);
            boxes.BoxesContainerSetup.RegisterLifeStyle(singletonAttr);

            boxes.BoxesIntegrationSetup.AddPackgeLevelFilter(new Filter(), "Identifiers");
        }
    }
}