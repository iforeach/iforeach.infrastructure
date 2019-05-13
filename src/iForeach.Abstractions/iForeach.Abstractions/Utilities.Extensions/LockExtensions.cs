using System;

namespace org.iForeach
{
    public static partial class LockExtensions
    {
        /// <summary>
        /// 条件锁定后执行方法
        /// </summary>
        /// <param name="locker"></param>
        /// <param name="condiction"></param>
        /// <param name="action"></param>
        public static void LockForCondition(this object locker, Func<bool> condiction, Action action)
        {
            if(condiction())
            {
                lock(locker)
                {
                    if(condiction())
                    {
                        action();
                    }
                }
            }
        }
    }
}
