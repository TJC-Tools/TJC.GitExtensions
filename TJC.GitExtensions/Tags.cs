namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static string[] GetTagList(string? searchPattern = null, string workingDirectory = ".")
    {
        var arguments = searchPattern is null
            ? new[] { "tag", "--list" }
            : new[] { "tag", "--list", searchPattern };
        return SplitLines(RunGit(workingDirectory, arguments));
    }

    public static void CreateTag(string tag, string workingDirectory = ".")
    {
        RunGit(workingDirectory, "tag", tag);
    }

    private static string[] SplitLines(string output)
    {
        return string.IsNullOrEmpty(output)
            ? Array.Empty<string>()
            : output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
    }
}