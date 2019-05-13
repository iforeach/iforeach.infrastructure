using System;
using Microsoft.Extensions.FileProviders;

namespace org.iForeach.Environment.Extensions.Hosting
{
    /// <summary>
    /// 接口 <c>Microsoft.Extensions.Hosting.IHostEnvironment</c> 的代理接口定义
    /// </summary>
    public interface IHostEnvironmentProxy
    {
        #region <c>Microsoft.Extensions.Hosting.IHostEnvironment</c> 接口代理

        /// <summary>
        /// <c>Microsoft.Extensions.Hosting.IHostEnvironment.EnvironmentName</c>
        /// </summary>
        string EnvironmentName { get; set; }

        /// <summary>
        /// <c>Microsoft.Extensions.Hosting.IHostEnvironment.ApplicationName</c>
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// <c>Microsoft.Extensions.Hosting.IHostEnvironment.ContentRootPath</c>
        /// </summary>
        string ContentRootPath { get; set; }

        /// <summary>
        /// <c>Microsoft.Extensions.Hosting.IHostEnvironment.ContentRootFileProvider</c>
        /// </summary>
        IFileProvider ContentRootFileProvider { get; set; }

        #endregion

        #region 补充常量定义

        /// <summary>
        /// 应用主模块名，默认为 <c>Application</c>
        /// </summary>
        string ApplicationModuleName { get; }

        /// <summary>
        /// 模块存储路径，默认为 <c>areas</c>
        /// </summary>
        string ModulesPath { get; }

        /// <summary>
        /// 应用入口路径，Web应用对应于 <c>wwwroot</c>
        /// </summary>
        string ApplicationEntryPath { get; }

        #endregion
    }

    /// <summary>
    /// 宿主环境代理封包，建议使用临时策略注入到依赖系统
    /// </summary>
    public sealed class HostEnvironmentProxyWrapper : IHostEnvironmentProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        public HostEnvironmentProxyWrapper(IHostEnvironmentProxy proxy)
        {
            this.Proxy = proxy;
        }

        /// <summary>
        /// 
        /// </summary>
        private IHostEnvironmentProxy Proxy
        {
            get;
        }

        /// <summary>
        /// <see cref="IHostEnvironmentProxy.ModulesPath"/>
        /// </summary>
        public string ModulesPathRoot
            => this.Proxy.ModulesPath.EnsureWithFolderSuffix();

        /// <summary>
        /// <see cref="IHostEnvironmentProxy.ApplicationEntryPath"/>
        /// </summary>
        public string ApplicationEntryPathRoot
            => this.Proxy.ApplicationEntryPath.EnsureWithFolderSuffix();

        /// <summary>
        /// <see cref="IHostEnvironmentProxy.EnvironmentName"/>
        /// </summary>
        /// <exception cref="NotSupportedException">when set</exception>
        public string EnvironmentName
        {
            get => this.Proxy.EnvironmentName;
            set => throw new NotSupportedException($"Please use {nameof(IHostEnvironmentProxy)} to set.");
        }

        /// <summary>
        /// <see cref="IHostEnvironmentProxy.ApplicationName"/>
        /// </summary>
        /// <exception cref="NotSupportedException">when set</exception>
        public string ApplicationName
        {
            get => this.Proxy.ApplicationName;
            set => throw new NotSupportedException($"Please use {nameof(IHostEnvironmentProxy)} to set.");
        }

        /// <summary>
        /// <see cref="IHostEnvironmentProxy.ContentRootPath"/>
        /// </summary>
        /// <exception cref="NotSupportedException">when set</exception>
        public string ContentRootPath
        {
            get => this.Proxy.ContentRootPath;
            set => throw new NotSupportedException($"Please use {nameof(IHostEnvironmentProxy)} to set.");
        }

        /// <summary>
        /// <see cref="IHostEnvironmentProxy.ContentRootFileProvider"/>
        /// </summary>
        /// <exception cref="NotSupportedException">when set</exception>
        public IFileProvider ContentRootFileProvider
        {
            get => this.Proxy.ContentRootFileProvider;
            set => throw new NotSupportedException($"Please use {nameof(IHostEnvironmentProxy)} to set.");
        }

        /// <summary>
        /// <see cref="IHostEnvironmentProxy.ApplicationModuleName"/>
        /// </summary>
        public string ApplicationModuleName => this.Proxy.ApplicationModuleName;

        /// <summary>
        /// <see cref="IHostEnvironmentProxy.ModulesPath"/> with folder suffix.
        /// </summary>
        public string ModulesPath => this.Proxy.ModulesPath;

        /// <summary>
        /// <see cref="IHostEnvironmentProxy.ApplicationEntryPath"/> with folder suffix.
        /// </summary>
        public string ApplicationEntryPath => this.Proxy.ApplicationEntryPath;
    }
}
