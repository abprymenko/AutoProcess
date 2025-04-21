using log4net.Config;

namespace AutoProcess.Tests;

#region TestSetup
[SetUpFixture]
public class TestSetup
{
    #region Init
    [OneTimeSetUp]
    public void Init()
    {
        var logConfigPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Files\log4net.config");
        XmlConfigurator.Configure(new FileInfo(logConfigPath));
    }
    #endregion
}
#endregion