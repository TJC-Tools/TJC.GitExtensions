namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static void Commit(
        string message,
        string workingDirectory = ".",
        GitCommandSettings? settings = null
    )
    {
        settings ??= new GitCommandSettings();
        var directories = GetRunDirectories(workingDirectory, settings.RunType);
        var singleRepositorySettings = new GitCommandSettings
        {
            DryRun = settings.DryRun,
            RunType = GitCommandRunType.Parent,
        };

        if (settings.DryRun)
        {
            foreach (var directory in directories)
            {
                RunGit(directory, singleRepositorySettings, "commit", "--all", "-m", message);
            }

            return;
        }

        foreach (var directory in directories.AsEnumerable().Reverse())
        {
            RunGit(directory, singleRepositorySettings, "add", "--all");
            RunGit(directory, singleRepositorySettings, "commit", "-m", message);
        }
    }
}
