using AutoProcess.Contracts.BusinessObjects.Models;

namespace AutoProcess.BusinessObjects.Models;

#region ProcessResult
internal class ProcessResult : IProcessResult
{
    public int ExitCode { get; set; }
    public string? StandardOutput { get; set; }
    public string? StandardError { get; set; }
}
#endregion