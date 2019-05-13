using System;

namespace org.iForeach
{
    /// <summary>
    /// 一致性数据助手：返回一致性的系统数据
    /// </summary>
    public static class CoincidentsHelper
    {
        /// <summary>
        /// 一致性的服务工厂静态实例
        /// </summary>
        public static ICoincidentsFactory Factory
        {
            get;
            private set;
        }

        /// <summary>
        /// 设置静态的一致性服务工厂静态实例
        /// </summary>
        /// <param name="factory"></param>
        public static void SetFactory(ICoincidentsFactory factory)
        {
            Factory = factory;
        }

        private static ICoincidentsFactory NullForDefault => Factory ?? CoincidentsDefault.Default;

        /// <summary>
        /// 获取目标服务
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <returns></returns>
        public static TService Service<TService>()
            where TService : class
            => Factory?.GetService<TService>() ?? default;

        /// <summary>
        /// 获取一致性的当前日期时间
        /// </summary>
        public static DateTime Now
            => NullForDefault.DatetimeGenerator.Now;

        /// <summary>
        /// 获取一致性的国际日期时间
        /// </summary>
        public static DateTime UtcNow
            => NullForDefault.DatetimeGenerator.UtcNow;

        /// <summary>
        /// 获取一致性的 <see cref="Guid"/> 值
        /// </summary>
        /// <returns></returns>
        public static Guid NewGuid()
            => NullForDefault.GuidGenerator.NewGuid();

        #region Default

        private class CoincidentsDefault : ICoincidentsFactory, IDatetimeCoincident, IGuidCoincident
        {
            public static readonly ICoincidentsFactory Default = new CoincidentsDefault();

            private CoincidentsDefault()
            {

            }

            IGuidCoincident ICoincidentsFactory.GuidGenerator => this;

            IDatetimeCoincident ICoincidentsFactory.DatetimeGenerator => this;

            DateTime IDatetimeCoincident.Now => DateTime.Now;

            DateTime IDatetimeCoincident.UtcNow => DateTime.UtcNow;

            TService ICoincidentsFactory.GetService<TService>()
            {
                throw new NotImplementedException();
            }

            Guid IGuidCoincident.NewGuid()
                => Guid.NewGuid();
        }

        #endregion
    }
}
