using Domain.Results;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace Application.Extensions
{
    /// <summary>
    /// ILogger extensions class
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Log Enter into method 
        /// </summary>
        public static void LogEnter(this ILogger logger, [CallerMemberName] string callerName = "")
        {
            logger.LogInformation($"Executing {callerName}");
        }

        /// <summary>
        /// Log Exit from a method
        /// </summary>
        public static void LogExit(this ILogger logger, Result result, [CallerMemberName] string callerName = "")
        {
            if (result.Error is Error.Success)
            {
                logger.LogInformation($"Executing method - {callerName} successfully");
            }
            else
            {
                logger.LogError($"Failure to execute method - {callerName}. Error type {result.Error}. Error message {result.Message}");
            }
        }

        /// <summary>
        /// Log Exit from a method
        /// </summary>
        public static void LogExit<TResult>(this ILogger logger, Result<TResult> result, [CallerMemberName] string callerName = "")
        {
            if (result.Error is Error.Success)
            {
                logger.LogInformation($"Executing method - {callerName} successfully");
            }
            else
            {
                logger.LogError($"Failure to execute method - {callerName}. Error type {result.Error}. Error message {result.Message}");
            }
        }
    }
}