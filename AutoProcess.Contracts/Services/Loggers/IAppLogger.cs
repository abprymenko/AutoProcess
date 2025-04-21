using System.Runtime.CompilerServices;

namespace AutoProcess.Contracts.Services.Loggers;

#region IAppLogger
public interface IAppLogger<in T> where T : class
{
    /// <summary>
    ///     Logs a debug message.
    /// </summary>
    /// <param name="message"></param>
    void Debug(string message, [CallerMemberName] string caller = "");
    /// <summary>
    ///    Logs an informational message.
    /// </summary>
    /// <param name="message"></param>
    void Info(string message, [CallerMemberName] string caller = "");
    /// <summary>
    ///    Logs a warning message.
    /// </summary>
    /// <param name="message"></param>
    void Warn(string message, [CallerMemberName] string caller = "");
    /// <summary>
    ///     Logs an error message with an optional exception.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="caller"></param>
    /// <param name="ex">Pass the exception if you want the system trace, otherwise pass just <c>null</c>.</param>
    void Error(string message, [CallerMemberName] string caller = "", Exception? ex = null);
}
#endregion