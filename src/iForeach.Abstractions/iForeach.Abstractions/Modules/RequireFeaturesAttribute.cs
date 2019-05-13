using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace org.iForeach.Modules
{
    /// <summary>
    /// When used on a class, it will include the service only
    /// if the specific features are enabled.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RequireFeaturesAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureName"></param>
        public RequireFeaturesAttribute(string featureName)
        {
            this.RequiredFeatureNames = new string[] { featureName };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureName"></param>
        /// <param name="otherFeatureNames"></param>
        public RequireFeaturesAttribute(string featureName, params string[] otherFeatureNames)
        {
            this.RequiredFeatureNames = new List<string>(otherFeatureNames) { featureName };
        }

        /// <summary>
        /// The names of the required features.
        /// </summary>
        public IList<string> RequiredFeatureNames { get; }

        /// <summary>
        /// 获取指定类型依赖的特性名称集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<string> GetRequiredFeatureNamesForType(Type type)
        {
            return type?.GetCustomAttributes<RequireFeaturesAttribute>(false)
                       ?.SingleOrDefault()
                       ?.RequiredFeatureNames
                       ?? Array.Empty<string>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static partial class FeatureExtensions
    {
        /// <summary>
        /// <seealso cref="RequireFeaturesAttribute.GetRequiredFeatureNamesForType(Type)"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IList<string> GetRequiredFeatureNames(this Type type)
        {
            return RequireFeaturesAttribute.GetRequiredFeatureNamesForType(type);
        }
    }
}