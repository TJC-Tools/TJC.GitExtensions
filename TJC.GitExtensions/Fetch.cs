namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static GitCommandResult Fetch(
        string workingDirectory = ".",
        GitCommandSettings? settings = null
    )
    {
        return RunGitCommand(workingDirectory, settings, GitDryRunMode.Supported, "fetch");
    }
}
