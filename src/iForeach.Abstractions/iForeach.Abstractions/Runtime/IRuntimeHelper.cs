using System;

namespace org.iForeach.Runtime
{
    /// <summary>
    /// 运行时助手接口定义，基于运行时的静态服务支持，可以类比为一个提供静态服务支持的 <c>Factory</c>
    /// </summary>
    public interface IRuntimeHelper
    {
        /// <summary>
        /// 基于当前服务上下文的 <see cref="IServiceContext"/>
        /// </summary>
        /// <remarks>
        /// <para>属性的值不应该缓存到 <see cref="IRuntimeHelper"/> 实例中，除非对应实现有特别需求。</para>
        /// <para>每次使用该属性，都应该通过 <see cref="IServiceProvider.GetService{TService}" />全新获取。</para>
        /// </remarks>
        IServiceContext ServiceContext { get; }
    }
}
