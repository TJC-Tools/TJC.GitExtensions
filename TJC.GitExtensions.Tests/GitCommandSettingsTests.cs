using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    
    public class GitCommandSettingsTests
    {
        [Fact]
        public void RunTypeUsesBinaryBackedValues()
        {
            Assert.Equal(1, (int)GitCommandRunType.Parent);
            Assert.Equal(2, (int)GitCommandRunType.Submodules);
            Assert.Equal(3, (int)GitCommandRunType.ParentAndSubmodules);
        }

        [Fact]
        public void DryRunDoesNotCreateCommit()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            var initialHash = GitExtensions.GetInformation(repository.Path).CommitHash;
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "changed");

            GitExtensions.Commit(
                "dry run",
                repository.Path,
                new GitCommandSettings { DryRun = true }
            );

            Assert.Equal(initialHash, GitExtensions.GetInformation(repository.Path).CommitHash);
            Assert.True(GitExtensions.GetInformation(repository.Path).IsDirty);
        }

        [Fact]
        public void ParentAndSubmodulesRunsCommandsInBothRepositories()
        {
            using var repository = TestRepository.Create();
            var submodulePath = repository.CreateSubmodule("child");
            repository.Commit("setup");
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "parent change");
            File.AppendAllText(Path.Combine(submodulePath, "submodule.txt"), "child change");

            var result = GitExtensions.Commit(
                "scoped commit",
                repository.Path,
                new GitCommandSettings { RunType = GitCommandRunType.ParentAndSubmodules }
            );

            Assert.True(result.Succeeded);
            Assert.Equal(0, result.ExitCode);
            Assert.False(GitExtensions.GetInformation(repository.Path).IsDirty);
            Assert.False(GitExtensions.GetInformation(submodulePath).IsDirty);
        }
    }
}
