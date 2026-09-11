namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static string GetBranchName(string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        return RunGit(workingDirectory, settings, "branch", "--show-current");
    }

    public static void Checkout(string branchName, string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        RunGit(workingDirectory, settings, "checkout", branchName);
    }

    public static void ChangeBranch(string branchName, string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        Checkout(branchName, workingDirectory, settings);
    }
}