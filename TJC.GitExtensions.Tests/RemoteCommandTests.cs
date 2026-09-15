using System.Diagnostics;
using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests;

[TestClass]
public class RemoteCommandTests
{
    [TestMethod]
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

            Assert.IsTrue(GitExtensions.Push(repository.Path).Succeeded);
            Assert.IsTrue(GitExtensions.Fetch(repository.Path).Succeeded);
            Assert.IsTrue(GitExtensions.Pull(repository.Path).Succeeded);
        }
        finally
        {
            foreach (var file in Directory.EnumerateFiles(remotePath, "*", SearchOption.AllDirectories))
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
        Assert.AreEqual(0, process.ExitCode);
    }
}