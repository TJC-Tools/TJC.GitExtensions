using System.Diagnostics;

namespace TJC.GitExtensions.Tests
{
    internal sealed class TestRepository : IDisposable
    {
        private TestRepository(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public static TestRepository Create()
        {
            var repository = new TestRepository(Directory.CreateTempSubdirectory().FullName);
            repository.Run("init");
            repository.Run("config", "user.email", "tests@example.com");
            repository.Run("config", "user.name", "Tests");
            return repository;
        }

        public void Commit(string message)
        {
            File.WriteAllText(System.IO.Path.Combine(Path, "file.txt"), message);
            Run("add", ".");
            Run("commit", "-m", message);
        }

        public void Tag(string tag)
        {
            Run("tag", tag);
        }

        public void CreateBranch(string branch)
        {
            Run("branch", branch);
        }

        public void Checkout(string branch)
        {
            Run("checkout", branch);
        }

        public void SetUpstream(string branch)
        {
            Run("branch", "--set-upstream-to", branch);
        }

        public string CreateSubmodule(string name)
        {
            var submodulePath = System.IO.Path.Combine(Path, name);
            Directory.CreateDirectory(submodulePath);
            RunAt(submodulePath, "init");
            RunAt(submodulePath, "config", "user.email", "tests@example.com");
            RunAt(submodulePath, "config", "user.name", "Tests");
            File.WriteAllText(System.IO.Path.Combine(submodulePath, "submodule.txt"), "initial");
            RunAt(submodulePath, "add", ".");
            RunAt(submodulePath, "commit", "-m", "initial");
            File.WriteAllText(
                System.IO.Path.Combine(Path, ".gitmodules"),
                $"[submodule \"{name}\"]{Environment.NewLine}\tpath = {name}{Environment.NewLine}\turl = https://example.com/{name}.git{Environment.NewLine}"
            );
            return submodulePath;
        }

        public void Dispose()
        {
            foreach (var file in Directory.EnumerateFiles(Path, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(file, FileAttributes.Normal);
            }

            Directory.Delete(Path, recursive: true);
        }

        private void Run(params string[] arguments)
        {
            RunAt(Path, arguments);
        }

        private static void RunAt(string workingDirectory, params string[] arguments)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                },
            };

            foreach (var argument in arguments)
            {
                process.StartInfo.ArgumentList.Add(argument);
            }

            process.Start();
            process.WaitForExit();
            Assert.AreEqual(0, process.ExitCode, process.StandardError.ReadToEnd());
        }
    }
}
