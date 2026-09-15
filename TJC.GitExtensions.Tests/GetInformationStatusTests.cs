using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    
    public class GetInformationStatusTests
    {
        [Fact]
        public void GetInformation_ReportsDirtyStatus()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "changed");

            var information = GitExtensions.GetInformation(repository.Path);

            Assert.True(information.IsDirty);
        }
    }
}
