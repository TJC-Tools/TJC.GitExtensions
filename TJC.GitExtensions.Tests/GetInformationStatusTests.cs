using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class GetInformationStatusTests
    {
        [TestMethod]
        public void GetInformation_ReportsDirtyStatus()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            File.AppendAllText(Path.Combine(repository.Path, "file.txt"), "changed");

            var information = GitExtensions.GetInformation(repository.Path);

            Assert.IsTrue(information.IsDirty);
        }
    }
}
