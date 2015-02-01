namespace Aspects.Windsor
{
    using Boxes.Integration.Extensions;
    using Boxes.Integration.Setup;
    using Boxes.Integration.Setup.Interception;
    using Castle.Windsor;

    public class Extensions : ISetupBoxesExtension<IDefaultContainerSetup<IWindsorContainer>>
    {
        public bool CanHandle(IDefaultContainerSetup<IWindsorContainer> extension)
        {
            return true;
        }

        public void Configure(IDefaultContainerSetup<IWindsorContainer> config)
        {
            config.AddInterception(new RegisterInterception()
                .Apply<LoggingInterceptor>()
                .Where(ctx => ctx.Service.Name.EndsWith("Command")));            
        }
    }
}
