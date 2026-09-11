namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static string GetBranchName(string workingDirectory = ".")
    {
        return RunGit(workingDirectory, "branch", "--show-current");
    }

    public static void Checkout(string branchName, string workingDirectory = ".")
    {
        RunGit(workingDirectory, "checkout", branchName);
    }

    public static void ChangeBranch(string branchName, string workingDirectory = ".")
    {
        Checkout(branchName, workingDirectory);
    }
}
