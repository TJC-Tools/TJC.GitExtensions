namespace TJC.GitExtensions;

public sealed record GitBranchInfo(
    string Name,
    bool IsCurrent,
    bool IsRemote,
    string? TrackingBranch
);
