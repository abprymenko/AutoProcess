namespace AutoProcess.Shared.Constants;

#region Codes
public enum Codes : uint
{
    /// <summary>
    ///    The process completed successfully. (0)
    /// </summary>
    Success = 0x00000000,
    /// <summary>
    ///     The process is already up to date or no updates are available. 
    ///     For Edge, Telegram (winget): -1978335189
    /// </summary>
    UP_TO_DATE_WINGET_I = 0x8A15002B,
    /// <summary>
    ///    The process is already up to date or no updates are available. 
    ///    For Chrome, Zoom (winget): -1978335212
    /// </summary>
    UP_TO_DATE_WINGET_II = 0x8a150014,
    /// <summary>
    ///    The process is already up to date or no updates are available.
    ///    For Chrome(GoogleUpdate.exe), ... : 75009
    /// </summary>
    UP_TO_DATE_USEUPDATER = 75009,
    /// <summary>
    ///     There is some failure in installation, which may include issues with the source from which a 
    ///     process is being downloaded or installed. (-2147220975)
    /// </summary>
    INSTALLATION_SOURCE_ERROR = 0x80040C01,
    /// <summary>
    ///     Stack buffer overrun detected. (-1073740791)
    /// </summary>
    STATUS_STACK_BUFFER_OVERRUN = 0xC0000409
}
#endregion