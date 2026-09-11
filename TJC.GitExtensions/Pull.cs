namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static GitCommandResult Pull(
        string workingDirectory = ".",
        GitCommandSettings? settings = null
    )
    {
        return RunGitResult(workingDirectory, settings, "pull");
    }
}
