using System.Collections.Generic;
using Boxes.Integration.Extensions;

namespace Process
{
    public interface ILoadProcess<TBuilder, TContainer> : IBoxesExtension
    {
        void LoadPackages(Application context, IEnumerable<string> packagesToEnable);
    }
}