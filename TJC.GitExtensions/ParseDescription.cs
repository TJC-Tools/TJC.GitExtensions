using System.Text.RegularExpressions;

namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    private static readonly Regex DescribeWithTag = new(
        "^(?<tag>.+)-(?<distance>[0-9]+)-g(?<hash>[0-9a-fA-F]+)$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex Version = new(
        "^[vV]?(?<major>[0-9]+)\\.(?<minor>[0-9]+)\\.(?<patch>[0-9]+)(?:[-+].*)?$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static GitDescription ParseDescription(string describe)
    {
        var describeMatch = DescribeWithTag.Match(describe);
        var tag = describeMatch.Success ? describeMatch.Groups["tag"].Value : null;
        var versionMatch = tag is null ? null : Version.Match(tag);

        return new GitDescription
        {
            VersionMatch = versionMatch,
            CommitHash = describeMatch.Success
                ? describeMatch.Groups["hash"].Value
                : describe,
            DistanceToLatestTag = describeMatch.Success
                ? int.Parse(describeMatch.Groups["distance"].Value)
                : null
        };
    }

    private static int? GetVersionPart(Match? versionMatch, string groupName)
    {
        return versionMatch?.Success == true
            ? int.Parse(versionMatch.Groups[groupName].Value)
            : null;
    }

    private sealed record GitDescription
    {
        public Match? VersionMatch { get; init; }

        public string CommitHash { get; init; } = string.Empty;

        public int? DistanceToLatestTag { get; init; }
    }
}