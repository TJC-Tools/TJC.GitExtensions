using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class CommitTests
    {
        [TestMethod]
        public void CommitStagesAllChanges()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "changed");

            GitExtensions.Commit("second", repository.Path);

            Assert.IsFalse(GitExtensions.GetInformation(repository.Path).IsDirty);
        }
    }
}