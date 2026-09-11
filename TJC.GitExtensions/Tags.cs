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

    public static GitCommandResult CreateTag(
        string tag,
        string workingDirectory = ".",
        GitCommandSettings? settings = null
    )
    {
        return RunGitResult(workingDirectory, settings, "tag", tag);
    }

    private static string[] SplitLines(string output)
    {
        return string.IsNullOrEmpty(output)
            ? Array.Empty<string>()
            : output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
    }
}
