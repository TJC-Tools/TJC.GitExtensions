using System.Diagnostics;
using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests;

public class RemoteCommandTests
{
    [Fact]
    public void PushFetchAndPull_OperateAgainstLocalRemote()
    {
        using var repository = TestRepository.Create();
        var remotePath = Directory.CreateTempSubdirectory().FullName;

        try
        {
            RunGit(remotePath, "init", "--bare");
            repository.Commit("initial");
            var branch = GitExtensions.GetBranchName(repository.Path);
            repository.AddRemote("origin", remotePath);
            repository.ConfigureTrackingBranch(branch, "origin");

            Assert.True(GitExtensions.Push(repository.Path).Succeeded);
            Assert.True(GitExtensions.Fetch(repository.Path).Succeeded);
            Assert.True(GitExtensions.Pull(repository.Path).Succeeded);
        }
        finally
        {
            foreach (
                var file in Directory.EnumerateFiles(remotePath, "*", SearchOption.AllDirectories)
            )
                File.SetAttributes(file, FileAttributes.Normal);
            Directory.Delete(remotePath, recursive: true);
        }
    }

    private static void RunGit(string workingDirectory, params string[] arguments)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "git",
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
            },
        };
        foreach (var argument in arguments)
            process.StartInfo.ArgumentList.Add(argument);

        process.Start();
        process.WaitForExit();
        Assert.Equal(0, process.ExitCode);
    }
}
