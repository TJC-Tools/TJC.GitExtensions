namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static string[] GetChangesList(string workingDirectory = ".")
    {
        var output = RunGit(workingDirectory, "status", "--short");
        return string.IsNullOrEmpty(output)
            ? Array.Empty<string>()
            : output.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    }

    private static readonly string[] separator = new[] { "\r\n", "\n" };
}
