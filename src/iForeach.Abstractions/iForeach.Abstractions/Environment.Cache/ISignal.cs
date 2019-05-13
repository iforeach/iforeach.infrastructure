using Microsoft.Extensions.Primitives;

namespace org.iForeach.Environment.Cache
{
    /// <summary>
    /// 信号处理接口
    /// </summary>
    public interface ISignal
    {
        /// <summary>
        /// 获取一个指定标识值的信号令牌
        /// </summary>
        /// <param name="key">信号令牌识别键</param>
        /// <returns></returns>
        IChangeToken GetToken(string key);

        /// <summary>
        /// 向指定标识键值的信号发送消息
        /// </summary>
        /// <param name="key">目标信号标识键值</param>
        void SignalToken(string key);
    }
}
