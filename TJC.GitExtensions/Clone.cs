namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static GitCommandResult Clone(
        string remoteUri,
        string localRepoPath,
        GitCommandSettings? settings = null
    )
    {
        var destination = Path.GetFullPath(localRepoPath);
        var parentDirectory =
            Path.GetDirectoryName(destination)
            ?? throw new ArgumentException(
                "The repository path must include a parent directory.",
                nameof(localRepoPath)
            );

        return RunGitCommand(
            parentDirectory,
            settings,
            GitDryRunMode.None,
            "clone",
            remoteUri,
            destination
        );
    }
}
