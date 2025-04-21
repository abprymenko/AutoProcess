namespace AutoProcess.Contracts.Core.Verifiers;

#region IArgumentValidator
public interface IArgumentValidator
{
    /// <summary>
    ///     Validates the process arguments.
    /// </summary>
    /// <param name="arguments"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    void ValidateProcessArguments(params string[]? arguments);
    /// <summary>
    ///     Validates the file exists and returns the file version.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    Task<string?> ValidateFileExists(string fileName);
}
#endregion