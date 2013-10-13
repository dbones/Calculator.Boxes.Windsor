using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boxes.Integration.Extensions;
using Boxes.Integration.Factories;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;

namespace Container.Windsor
{
    public class SetupWindsor : IIocSetup<IWindsorContainer>, IBoxesExtension
    {
        public void Configure(IWindsorContainer builder)
        {
            builder.Kernel.Resolver.AddSubResolver(new CollectionResolver(builder.Kernel, true));
        }

        public void ConfigureChild(IWindsorContainer builder)
        {
            throw new NotImplementedException();
        }
    }
}
