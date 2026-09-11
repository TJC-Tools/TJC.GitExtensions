namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static GitRepositoryInfo GetInformation(
        string workingDirectory = ".",
        GitCommandSettings? settings = null
    )
    {
        var describe = RunGit(
            workingDirectory,
            settings,
            "describe",
            "--tags",
            "--always",
            "--long",
            "--abbrev=40"
        );
        var status = RunGit(workingDirectory, settings, "status", "--short");
        var description = ParseDescription(describe);

        return new GitRepositoryInfo
        {
            IsVersionNumber = description.VersionMatch?.Success == true,
            Major = GetVersionPart(description.VersionMatch, "major"),
            Minor = GetVersionPart(description.VersionMatch, "minor"),
            Patch = GetVersionPart(description.VersionMatch, "patch"),
            IsDirty = !string.IsNullOrWhiteSpace(status),
            CommitHash = description.CommitHash,
            DistanceToLatestTag = description.DistanceToLatestTag,
        };
    }
}
