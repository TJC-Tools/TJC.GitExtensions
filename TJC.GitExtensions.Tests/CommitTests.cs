using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    public class CommitTests
    {
        [Fact]
        public void CommitStagesAllChanges()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "changed");

            GitExtensions.Commit("second", repository.Path);

            Assert.False(GitExtensions.GetInformation(repository.Path).IsDirty);
        }
    }
}
