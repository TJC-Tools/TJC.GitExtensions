using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class ChangesTests
    {
        [TestMethod]
        public void GetChangesListReturnsWorkingTreeChanges()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "changed");

            CollectionAssert.AreEqual(
                new[] { " M file.txt" },
                GitExtensions.GetChangesList(repository.Path));
        }
    }
}