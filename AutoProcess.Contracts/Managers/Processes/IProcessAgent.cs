using AutoProcess.Contracts.BusinessObjects.Models;
using System.Diagnostics;

namespace AutoProcess.Contracts.Managers.Processes;

#region IProcessAgent
public interface IProcessAgent
{
    /// <summary>
    ///   Beendet alle Prozesse mit den angegebenen Namen.
    /// </summary>
    /// <param name="processesNames"></param>
    /// <returns></returns>
    Task KillProcesses(params string[] processesNames);
    /// <summary>
    ///     Startet den Prozess mit den angegebenen Startinformationen.
    /// </summary>
    /// <param name="processStartInfo"></param>
    /// <returns></returns>
    Task<IProcessResult> UpdateProcess(ProcessStartInfo processStartInfo);
    /// <summary>
    ///     Startet den Prozess mit dem angegebenen Dateinamen.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task StartProcess(string fileName);
}
#endregion