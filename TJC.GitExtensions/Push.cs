namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static void Push(string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        RunGit(workingDirectory, settings, "push");
        RunGit(workingDirectory, settings, "push", "--tags");
    }
}
