namespace TJC.GitExtensions
{
    public sealed record GitRepositoryInfo
    {
        public bool IsVersionNumber { get; init; }

        public int? Major { get; init; }

        public int? Minor { get; init; }

        public int? Patch { get; init; }

        public bool IsDirty { get; init; }

        public string CommitHash { get; init; } = string.Empty;

        public int? DistanceToLatestTag { get; init; }
    }
}