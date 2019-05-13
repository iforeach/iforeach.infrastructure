using System;

namespace org.iForeach
{
    /// <summary>
    /// 一致性数据助手：返回一致性的系统数据
    /// </summary>
    public interface ICoincidentsFactory
    {
        /// <summary>
        /// 一致性的 <see cref="Guid"/> 生成器
        /// </summary>
        IGuidCoincident GuidGenerator
        {
            get;
        }

        /// <summary>
        /// 一致性的 <see cref="DateTime"/> 生成器
        /// </summary>
        IDatetimeCoincident DatetimeGenerator
        {
            get;
        }

        /// <summary>
        /// 获取一致性数据服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        TService GetService<TService>()
            where TService : class;
    }
}
