namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static string[] GetChangesList(
        string workingDirectory = ".",
        GitCommandSettings? settings = null
    )
    {
        var output = RunGit(workingDirectory, settings, "status", "--short");
        return string.IsNullOrEmpty(output)
            ? Array.Empty<string>()
            : output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
    }
}
