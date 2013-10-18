using System.Collections.Generic;
using Boxes.Integration;
using Boxes.Integration.Setup;

namespace Process
{
    /// <summary>
    /// the application level is to register components which are shared between tenants. components which are enabled at this level should be able to handle tenancy.
    /// </summary>
    public class Application
    {
        /// <summary>
        /// the packages the application requires (this does not mean they could be enabled)
        /// </summary>
        public IEnumerable<string> Packages { get; set; }

        /// <summary>
        /// the enabled packages
        /// </summary>
        public IEnumerable<string> EnabledPackages { get; set; }

        /// <summary>
        /// the container for the applicaiton
        /// </summary>
        public object Container { get; set; }

    }
}