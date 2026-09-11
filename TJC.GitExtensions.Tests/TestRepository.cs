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
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    WorkingDirectory = Path,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
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