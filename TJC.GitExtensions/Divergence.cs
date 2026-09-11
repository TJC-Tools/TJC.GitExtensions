namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static int GetBehindCount(string workingDirectory = ".")
    {
        return GetDivergenceCounts(workingDirectory).Behind;
    }

    public static int GetAheadCount(string workingDirectory = ".")
    {
        return GetDivergenceCounts(workingDirectory).Ahead;
    }

    private static (int Ahead, int Behind) GetDivergenceCounts(string workingDirectory)
    {
        var counts = RunGit(
            workingDirectory,
            "rev-list",
            "--left-right",
            "--count",
            "HEAD...@{upstream}");
        var values = counts.Split('\t', StringSplitOptions.RemoveEmptyEntries);

        if (values.Length != 2 ||
            !int.TryParse(values[0], out var ahead) ||
            !int.TryParse(values[1], out var behind))
        {
            throw new InvalidOperationException($"Unexpected git divergence output: {counts}");
        }

        return (ahead, behind);
    }
}