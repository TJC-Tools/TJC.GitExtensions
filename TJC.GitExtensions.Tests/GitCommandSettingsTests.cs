using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class GitCommandSettingsTests
    {
        [TestMethod]
        public void RunTypeUsesBinaryBackedValues()
        {
            Assert.AreEqual(1, (int)GitCommandRunType.Parent);
            Assert.AreEqual(2, (int)GitCommandRunType.Submodules);
            Assert.AreEqual(3, (int)GitCommandRunType.ParentAndSubmodules);
        }

        [TestMethod]
        public void DryRunDoesNotCreateCommit()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            var initialHash = GitExtensions.GetInformation(repository.Path).CommitHash;
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "changed");

            GitExtensions.Commit(
                "dry run",
                repository.Path,
                new GitCommandSettings { DryRun = true });

            Assert.AreEqual(initialHash, GitExtensions.GetInformation(repository.Path).CommitHash);
            Assert.IsTrue(GitExtensions.GetInformation(repository.Path).IsDirty);
        }

        [TestMethod]
        public void ParentAndSubmodulesRunsCommandsInBothRepositories()
        {
            using var repository = TestRepository.Create();
            var submodulePath = repository.CreateSubmodule("child");
            repository.Commit("setup");
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "parent change");
            File.AppendAllText(Path.Combine(submodulePath, "submodule.txt"), "child change");

            GitExtensions.Commit(
                "scoped commit",
                repository.Path,
                new GitCommandSettings { RunType = GitCommandRunType.ParentAndSubmodules });

            Assert.IsFalse(GitExtensions.GetInformation(repository.Path).IsDirty);
            Assert.IsFalse(GitExtensions.GetInformation(submodulePath).IsDirty);
        }
    }
}