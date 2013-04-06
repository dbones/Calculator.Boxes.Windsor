namespace Identifiers
{
    /// <summary>
    /// an exportable service (no defined lifestyle)
    /// 
    /// Developer note, try not to create inheritance higherachy with the DependencyAttribute
    /// 
    /// Not recommended, ICustomIDependency : ITransientDependency : IDependency 
    /// Recommended, ICustomIDependency : IDependency
    /// 
    /// this will make the registration easier to handle.
    /// </summary>
    public interface IDependency { }

    public interface ISingletonDependency : IDependency { }

    public interface ITransientDependency : IDependency { }
}
