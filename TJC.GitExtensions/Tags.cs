namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static string[] GetTagList(
        string? searchPattern = null,
        string workingDirectory = ".",
        GitCommandSettings? settings = null
    )
    {
        var arguments = searchPattern is null
            ? new[] { "tag", "--list" }
            : new[] { "tag", "--list", searchPattern };
        return SplitLines(RunGit(workingDirectory, settings, arguments));
    }

    public static void CreateTag(
        string tag,
        string workingDirectory = ".",
        GitCommandSettings? settings = null
    )
    {
        RunGit(workingDirectory, settings, "tag", tag);
    }

    private static string[] SplitLines(string output)
    {
        return string.IsNullOrEmpty(output)
            ? Array.Empty<string>()
            : output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
    }
}
