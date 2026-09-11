namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static void Pull(string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        RunGit(workingDirectory, settings, "pull");
    }
}
