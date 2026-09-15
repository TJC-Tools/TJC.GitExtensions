using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class ChangesTests
    {
        private static readonly string[] expected = new[] { " M file.txt" };

        [TestMethod]
        public void GetChangesListReturnsWorkingTreeChanges()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "changed");

            CollectionAssert.AreEqual(expected, GitExtensions.GetChangesList(repository.Path));
        }
    }
}
