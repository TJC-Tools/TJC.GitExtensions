namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static GitCommandResult Push(
        string workingDirectory = ".",
        GitCommandSettings? settings = null
    )
    {
        var branchResult = RunGitCommand(
            workingDirectory,
            settings,
            GitDryRunMode.Supported,
            "push"
        );
        var tagResult = RunGitCommand(
            workingDirectory,
            settings,
            GitDryRunMode.Supported,
            "push",
            "--tags"
        );
        return CombineResults(new[] { branchResult, tagResult });
    }
}
