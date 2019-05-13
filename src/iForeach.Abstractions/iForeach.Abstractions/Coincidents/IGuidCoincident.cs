using System;

namespace org.iForeach
{
    /// <summary>
    /// 一致性的 <see cref="Guid"/> 数据
    /// </summary>
    public interface IGuidCoincident
    {
        /// <summary>
        /// 生成一个一致性的 <see cref="Guid"/> 值，默认：<seealso cref="Guid.NewGuid"/>
        /// </summary>
        /// <returns></returns>
        Guid NewGuid();
    }
}
