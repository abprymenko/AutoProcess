namespace AutoProcess.Contracts.BusinessObjects.Models;

#region IProcessResult
public interface IProcessResult
{
    /// <summary>
    ///   Exit code of the process.
    /// <list type="number">
    /// <item>
    /// <term>0</term>
    /// <description>success;</description>
    /// </item>
    /// <item>
    /// <term>1</term>
    /// <description>failed;</description>
    /// </item>
    /// <item>
    /// <term>2</term>
    /// <description>update pending.</description>
    /// </item>
    /// </list>
    /// </summary>
    int ExitCode { get; set; }
    /// <summary>
    ///   Standard output of the process, to see what's happening.
    /// </summary>
    string? StandardOutput { get; set; }
    /// <summary>
    ///   Standard error of the process.
    /// </summary>
    string? StandardError { get; set; }
}
#endregion