using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    
    public class ChangesTests
    {
        private static readonly string[] expected = new[] { " M file.txt" };

        [Fact]
        public void GetChangesListReturnsWorkingTreeChanges()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "changed");

            Assert.Equal(expected, GitExtensions.GetChangesList(repository.Path));
        }
    }
}
