namespace TJC.GitExtensions;

[Flags]
public enum GitCommandRunType
{
    Parent = 0b_0000_0001,
    Submodules = 0b_0000_0010,
    ParentAndSubmodules = Parent | Submodules
}

public sealed class GitCommandSettings
{
    public bool DryRun { get; init; }

    public GitCommandRunType RunType { get; init; } = GitCommandRunType.Parent;
}