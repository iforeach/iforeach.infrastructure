using System;

namespace org.iForeach.Runtime
{
    /// <summary>
    /// 服务上下文，用于取代Web应用的 <c>HttpContext</c>.
    /// </summary>
    /// <remarks>
    /// <para>该接口在 <c>IoC</c>的注入方式应该是 <c>Transident</c>.</para>
    /// <para>在接口最终实现上，应该实现一些其它需要用到的支持内容.</para>
    /// </remarks>
    /// <example>
    ///     1、作为 <c>HttpContext</c> 的取代接口，应该定义一个封装了 <c>HttpContext</c> 属性的代理接口：
    ///         <code>
    ///             public interface IHttpServiceContext : IServiceContext
    ///             {
    ///                 HttpContext HttpContext { get; }
    ///             }
    ///         </code>
    ///     2、作为提供了 <c>IHttpServiceContext</c> 实现的参考实现：
    ///         <code>
    ///             public class HttpServiceContext : IHttpServiceContext
    ///             {
    ///                 private readonly IServiceProvider _serviceProvider;
    ///                 public HttpServiceContext(IServiceProvider serviceProvider)
    ///                 {
    ///                     this._serviceProvider = serviceProvider;
    ///                 }
    ///                 
    ///                 public HttpContext HttpContext
    ///                     => this._serviceProvider.GetService(typeof(HttpContext)) as HttpContext;
    ///             }
    ///         </code>
    /// </example>
    public interface IServiceContext
    {
        /// <summary>
        /// 基于当前服务上下文的 <see cref="IServiceProvider"/>
        /// </summary>
        IServiceProvider ServiceProvider { get; }
    }

    /// <summary>
    /// <see cref="IServiceContext"/> 基本实现
    /// </summary>
    public abstract class ServiceContextBase : IServiceContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ServiceContextBase(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// <see cref="IServiceContext.ServiceProvider"/>
        /// </summary>
        protected IServiceProvider ServiceProvider
        {
            get;
        }

        IServiceProvider IServiceContext.ServiceProvider => this.ServiceProvider;
    }
}
