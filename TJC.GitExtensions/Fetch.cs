namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static void Fetch(string workingDirectory = ".", GitCommandSettings? settings = null)
    {
        RunGit(workingDirectory, settings, "fetch");
    }
}