namespace Identifiers.Windsor
{
    using System;
    using System.Linq;

    public static class AttributeExtensions
    {
        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), true).Any();
        }
    }
}
