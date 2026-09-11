namespace TJC.GitExtensions;

public static partial class GitExtensions
{
    public static void Fetch(string workingDirectory = ".")
    {
        RunGit(workingDirectory, "fetch");
    }
}
