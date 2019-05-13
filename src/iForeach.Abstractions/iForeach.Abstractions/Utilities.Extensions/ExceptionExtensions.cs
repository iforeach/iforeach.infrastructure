using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace org.iForeach
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ExceptionExtensions
    {
        private readonly static IList<Func<Exception, bool>> s_fatalExceptionTester = new List<Func<Exception, bool>>
                                                                                      {
                                                                                         ex => ex is OutOfMemoryException,
                                                                                         ex => ex is SecurityException,
                                                                                         ex => ex is SEHException,
                                                                                      };

        /// <summary>
        /// 判断<paramref name="ex"/>是否致命异常
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        /// <remarks>
        /// 致命异常包括 <see cref="OutOfMemoryException"/>、<see cref="SecurityException"/>、<see cref="SEHException"/>等。
        /// </remarks>
        public static bool IsFatal(this Exception ex)
        {
            return s_fatalExceptionTester.Any(tester => tester(ex));
        }
    }
}