namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static void Commit(string message, string workingDirectory = ".")
    {
        RunGit(workingDirectory, "add", "--all");
        RunGit(workingDirectory, "commit", "-m", message);
    }
}
