namespace Identifiers.Windsor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boxes;
    using Boxes.Integration.Setup;

    public class Filter : IPackageTypesFilter
    {
        public IEnumerable<Type> FilterTypes(Package package)
        {
            return Enumerable.Empty<Type>();
        }
    }
}