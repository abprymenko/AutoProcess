using AutoProcess.Tests.Managers;
using AutoProcess.Shared.Constants;
using AutoProcess.Shared.Properties;
using AutoProcess.Core.Helpers.Extensions.System;
using AutoProcess.Contracts.Managers.Processes;
using AutoProcess.Contracts.Services.Loggers;
using AutoProcess.Contracts.Core.Verifiers;
using System.Diagnostics;
using System.Text;

namespace AutoProcess.Tests.AutoProcess.Managers.Tests;

#region AutoProcess_Managers_Test
internal class AutoProcess_Managers_Test
{
    #region Private : Fields
    private IArgumentValidator _argumentValidator;
    private IProcessAgent _processManager;
    private IAppLogger<AutoProcess_Managers_Test> _logger;
    private static readonly string[][] BrowserArguments = [ 
                                                            //["msedge",
                                                            // @"C:\Program Files (x86)\Microsoft\EdgeUpdate\MicrosoftEdgeUpdate.exe", 
                                                            // "/checknow",
                                                            // @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"],
                                                            ["msedge", 
                                                             "winget", 
                                                             "upgrade Microsoft.Edge --silent",
                                                             @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"],
                                                            //["chrome", 
                                                            // @"C:\Program Files (x86)\Google\Update\GoogleUpdate.exe", 
                                                            // "/ua /installsource scheduler", 
                                                            // @"C:\Program Files\Google\Chrome\Application\chrome.exe"],
                                                            ["chrome", 
                                                             "winget", 
                                                             "upgrade --id Google.Chrome --silent --accept-package-agreements --accept-source-agreements", 
                                                             @"C:\Program Files\Google\Chrome\Application\chrome.exe" ]
                                                          ];
    private static readonly string[][] AppArguments = [ 
                                                         /*
                                                          * "notepad++", 
                                                          * @"C:\Program Files\Notepad++\updater\GUP.exe",
                                                          * "/checknow", 
                                                          * @"C:\Program Files\Notepad++\notepad++.exe"
                                                          */
                                                         ["notepad++", 
                                                          "winget",
                                                          "upgrade --id Notepad++.Notepad++ --silent --accept-package-agreements --accept-source-agreements",
                                                          @"C:\Program Files\Notepad++\notepad++.exe"],
                                                         /*
                                                          * "Telegram", 
                                                          * @"C:\Users\AndriiPrymenko\AppData\Roaming\Telegram Desktop\Updater.exe",
                                                          * "/silent", 
                                                          * @"C:\Users\AndriiPrymenko\AppData\Roaming\Telegram Desktop\Telegram.exe"
                                                          * */
                                                         ["telegram", 
                                                          "winget",
                                                          "upgrade --id Telegram.TelegramDesktop --silent --accept-package-agreements --accept-source-agreements",
                                                          @"C:\Users\AndriiPrymenko\AppData\Roaming\Telegram Desktop\Telegram.exe"],
                                                         /*
                                                          * Zoom downloadURL = "https://zoom.us/client/latest/ZoomInstallerFull.exe";
                                                          * Zoom path to save installer = @"C:\Temp\ZoomInstallerFull.exe";
                                                          * ---------------------------------------------------
                                                          * "Zoom", 
                                                          * @"C:\Temp\ZoomInstallerFull.exe",
                                                          * "/quiet /norestart", 
                                                          * @"C:\Users\AndriiPrymenko\AppData\Roaming\Zoom\bin\Zoom.exe"
                                                          * ---------------------------------------------------
                                                          */
                                                         ["zoom",
                                                          "winget",
                                                          "upgrade --id Zoom.Zoom --silent --accept-package-agreements --accept-source-agreements",
                                                          @"C:\Users\AndriiPrymenko\AppData\Roaming\Zoom\bin\Zoom.exe"]
                                                      ];
    #endregion

    #region Setup
    [SetUp]
    public void Setup()
    {
        _argumentValidator = Test_IoC.Container.Resolve<IArgumentValidator>();
        _processManager = Test_IoC.Container.Resolve<IProcessAgent>();
        _logger = Test_IoC.Container.Resolve<IAppLogger<AutoProcess_Managers_Test>>();
    }
    #endregion

    #region Test : Methods
    /// <summary>
    ///   Testet den Update-Prozess der Browser. 
    ///   Es wird überprüft, ob die Version des Browsers aktualisiert wurde. 
    ///   Es geht darum, den Prozess zu beenden, den Update-Prozess zu starten und dann die Version des Browsers zu überprüfen.
    ///   Es gibt einen Testfall für jeden Browser, der in der BrowserArguments-Variablen definiert ist.
    ///   z.B.: 1) Consumer Edge (personal PCs, home users):
    ///             MicrosoftEdgeUpdate.exe /checknow
    ///         2) Edge for Business (or general package update via Windows Update/Microsoft Store):
    ///             winget upgrade Microsoft.Edge
    ///      
    /// </summary>
    /// <param name="processName"></param>
    /// <param name="fileName"></param>
    /// <param name="arguments"></param>
    /// <param name="fileVersionPath"></param>
    /// <returns></returns>
    [TestCaseSource(nameof(BrowserSources))]
    [TestCaseSource(nameof(OtherAppSources))]
    public async Task UpdateApps_Test(string processName, string fileName, string arguments, string fileVersionPath)
    {
        try
        {
            _argumentValidator.ValidateProcessArguments(processName, fileName, arguments, fileVersionPath);
            var originalVersion = await _argumentValidator.ValidateFileExists(fileVersionPath);
            _logger.Debug(string.Format(Resources.Version, processName, originalVersion));
            await _processManager.KillProcesses(processName);
            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                //Verb = "runas",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };
            var processResult = await _processManager.UpdateProcess(processStartInfo);
            _logger.Debug(string.Format(Resources.Output, processResult.StandardOutput));
            _logger.Debug(string.Format(Resources.Error, processResult.StandardError));
            _logger.Debug(string.Format(Resources.ExitCode, processResult.ExitCode));
            if (processResult != null)
            {
                int exitCode = processResult.ExitCode;
                if (exitCode == Codes.Success.ToIntBitwise())
                {
                    var newVersion = await _argumentValidator.ValidateFileExists(fileVersionPath);
                    Assert.That(newVersion, Is.Not.EqualTo(originalVersion),
                                string.Format(Resources.SameVersions, originalVersion, newVersion));
                    _logger.Info(string.Format(Resources.SuccessUpdated, processName));
                }
                else if (exitCode == Codes.UP_TO_DATE_WINGET_I.ToIntBitwise()
                         || exitCode == Codes.UP_TO_DATE_WINGET_II.ToIntBitwise()
                         || exitCode == Codes.UP_TO_DATE_USEUPDATER.ToIntBitwise()
                         || (processResult.StandardOutput != null 
                             && processResult.StandardOutput.Contains(Resources.NoAvailableUpgrade)))
                {
                    _logger.Info(string.Format(Resources.IsUpToDate, processName));
                }
                else if (exitCode == Codes.INSTALLATION_SOURCE_ERROR.ToIntBitwise())
                {
                    Assert.Fail(string.Format(Resources.InstallationSourceError, processName, processResult.StandardError));
                }
                else if (exitCode == Codes.STATUS_STACK_BUFFER_OVERRUN.ToIntBitwise())
                {
                    Assert.Fail(Resources.StatusStackBufferOverrun);
                }
                else
                {
                    Assert.Fail(string.Format(Resources.DefaultInstallationError, processName));
                }
            }
            else
            {
                Assert.Fail(string.Format(Resources.DefaultInstallationError, processName));
            }
        }
        catch (Exception ex)
        {
            _logger.Error(string.Format(Resources.TestFailed, ex.Message));
        }
        finally
        {
            await _processManager.KillProcesses(processName);
        }
    }
    #endregion

    #region Internal : Properties
    internal static IEnumerable<TestCaseData> BrowserSources
    {
        get
        {
            foreach (var argument in BrowserArguments)
            {
                yield return new TestCaseData(argument).SetName(string.Format(Resources.Argument, argument[0]));
            }
        }
    }
    internal static IEnumerable<TestCaseData> OtherAppSources
    {
        get
        {
            foreach (var argument in AppArguments)
            {
                yield return new TestCaseData(argument).SetName(string.Format(Resources.Argument, argument[0]));
            }
        }
    }
    #endregion
}
#endregion