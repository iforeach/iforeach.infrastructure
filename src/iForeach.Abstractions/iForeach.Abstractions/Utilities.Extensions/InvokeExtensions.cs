using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace org.iForeach
{
    public static partial class InvokeExtensions
    {
        #region Invoke

        /// <summary>
        /// Safely invoke methods by catching non fatal exceptions and logging them
        /// </summary>
        public static void Invoke<TEvent>(this IEnumerable<TEvent> events,
                                          Action<TEvent> dispatch,
                                          ILogger logger)
        {
            events.Invoke((_, sink) => dispatch(sink),
                          logger,
                          default(IList<object>));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="events"></param>
        /// <param name="dispatch"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Invoke<TEvent, TResult>(this IEnumerable<TEvent> events,
                                                                   Func<TEvent, TResult> dispatch,
                                                                   ILogger logger)
        {
            return events.Invoke((list, sink) => list.Add(dispatch(sink)),
                                 logger,
                                 new List<TResult>());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="events"></param>
        /// <param name="dispatch"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Invoke<TEvent, TResult>(this IEnumerable<TEvent> events,
                                                                   Func<TEvent, IEnumerable<TResult>> dispatch,
                                                                   ILogger logger)
        {
            return events.Invoke((list, sink) => list.AddRange(dispatch(sink)),
                                 logger,
                                 new List<TResult>());
        }

        private static IList<TResult> Invoke<TEvent, TResult>(this IEnumerable<TEvent> events,
                                                              Action<IList<TResult>, TEvent> action,
                                                              ILogger logger,
                                                              IList<TResult> list = null)
        {
            foreach (var sink in events)
            {
                try
                {
                    action(list, sink);
                }
                catch (Exception ex)
                {
                    HandleException(ex,
                                    logger,
                                    typeof(TEvent).Name,
                                    sink.GetType().FullName);
                }
            }
            return list;
        }

        #endregion

        #region InvokeAsync

        /// <summary>
        /// Safely invoke methods by catching non fatal exceptions and logging them
        /// </summary>
        public static async Task InvokeAsync<TEvents>(this IEnumerable<TEvents> events,
                                                      Func<TEvents, Task> dispatch,
                                                      ILogger logger)
        {
            await events.InvokeAsync((list, sink) => Task.Run(() => dispatch(sink).GetAwaiter().GetResult()),
                                     logger,
                                     default(IList<object>));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvents"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="events"></param>
        /// <param name="dispatch"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TResult>> InvokeAsync<TEvents, TResult>(this IEnumerable<TEvents> events,
                                                                                     Func<TEvents, Task<TResult>> dispatch,
                                                                                     ILogger logger)
        {
            return await events.InvokeAsync((list, sink) => Task.Run(() => list.Add(dispatch(sink).GetAwaiter().GetResult())),
                                    logger,
                                    new List<TResult>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvents"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="events"></param>
        /// <param name="dispatch"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TResult>> InvokeAsync<TEvents, TResult>(this IEnumerable<TEvents> events,
                                                                                     Func<TEvents, Task<IEnumerable<TResult>>> dispatch,
                                                                                     ILogger logger)
        {
            return await events.InvokeAsync((list, sink) => Task.Run(() => list.AddRange(dispatch(sink).GetAwaiter().GetResult())),
                                            logger,
                                            new List<TResult>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="events"></param>
        /// <param name="action"></param>
        /// <param name="logger"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static async Task<IList<TResult>> InvokeAsync<TEvent, TResult>(this IEnumerable<TEvent> events,
                                                                               Func<IList<TResult>, TEvent, Task> action,
                                                                               ILogger logger,
                                                                               IList<TResult> list = null)
        {
            return await Task.Run(() => events.Invoke((list, sink) => action(list, sink).GetAwaiter()
                                                                                        .GetResult(),
                                                      logger,
                                                      list));
        }

        #endregion

        /// <summary>
        /// 执行异常处理
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="logger"></param>
        /// <param name="sourceType">触发异常的源类型</param>
        /// <param name="method">触发异常的方法名</param>
        /// <remarks>
        /// <para>当<paramref name="ex"/>是可记录异常时（非致命异常），执行日志记录操作。</para>
        /// <para>当<paramref name="ex"/>是致命异常时（<see cref="ExceptionExtensions.IsFatal(Exception)"/>），直接抛出异常。</para>
        /// </remarks>
        public static void HandleException(Exception ex,
                                           ILogger logger,
                                           string sourceType,
                                           string method)
        {
            switch(ex)
            {
                case var _ when CanbeLogged(ex):
                    logger.LogError(ex,
                                    "{Type} thrown from {Method} by {Exception}",
                                    sourceType,
                                    method,
                                    ex.GetType().Name);
                    break;
                case var _ when ex.IsFatal():
                    throw ex;
            }
        }

        private static bool CanbeLogged(Exception ex)
        {
            return !ex.IsFatal();
        }
    }
}