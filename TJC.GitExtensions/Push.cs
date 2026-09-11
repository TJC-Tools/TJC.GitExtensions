namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static void Push(string workingDirectory = ".")
    {
        RunGit(workingDirectory, "push");
        RunGit(workingDirectory, "push", "--tags");
    }
}
