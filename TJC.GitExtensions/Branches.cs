namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static GitBranchInfo[] GetBranchList(string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        var output = RunGit(
            workingDirectory,
            settings,
            "branch",
            "--all",
            "--format=%(refname:short)|%(HEAD)|%(upstream:short)");

        return output
            .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Split('|', 3))
            .Where(parts => parts.Length == 3)
            .Select(parts => new GitBranchInfo(
                parts[0],
                parts[1] == "*",
                parts[0].StartsWith("remotes/", StringComparison.Ordinal),
                string.IsNullOrEmpty(parts[2]) ? null : parts[2]))
            .ToArray();
    }

    public static string GetBranchName(string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        return RunGit(workingDirectory, settings, "branch", "--show-current");
    }

    public static GitCommandResult Checkout(string branchName, string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        return RunGitResult(workingDirectory, settings, "checkout", branchName);
    }

    public static GitCommandResult ChangeBranch(string branchName, string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        return Checkout(branchName, workingDirectory, settings);
    }
}