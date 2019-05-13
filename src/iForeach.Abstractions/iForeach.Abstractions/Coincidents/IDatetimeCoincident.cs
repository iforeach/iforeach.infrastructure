using System;

namespace org.iForeach
{
    /// <summary>
    /// 一致性数据助手：返回一致性的系统数据
    /// </summary>
    public interface IDatetimeCoincident

    {
        /// <summary>
        /// 获取一个一致性的 <see cref="DateTime"/> 值，默认：<seealso cref="DateTime.Now"/>
        /// </summary>
        DateTime Now
        {
            get;
        }

        /// <summary>
        /// 获取一个一致性的 <see cref="DateTime"/> 值，默认：<seealso cref="DateTime.UtcNow"/>
        /// </summary>
        DateTime UtcNow
        {
            get;
        }
    }
}
