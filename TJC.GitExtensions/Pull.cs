namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static void Pull(string workingDirectory = ".")
    {
        RunGit(workingDirectory, "pull");
    }
}
