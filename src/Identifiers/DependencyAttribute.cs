namespace Identifiers
{
    using System;

    /// <summary>
    /// an exportable service (no defined lifestyle)
    /// 
    /// Developer note, try not to create inheritance higherachy with the DependencyAttribute
    /// 
    /// Not recommended, CustomDependencyAttribute : TransientDependencyAttribute : DependencyAttribute 
    /// Recommended, ICustomDependency : IDependency
    /// 
    /// this will make the registration easier to handle.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DependencyAttribute : Attribute { }

    public class SingletonDependencyAttribute : DependencyAttribute { }

    public class TransientDependencyAttribute : DependencyAttribute { }
}
