using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    public class TagTests
    {
        [Fact]
        public void TagCommandsCreateAndListTags()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");

            GitExtensions.CreateTag("v1.0.0", repository.Path);

            Assert.Contains("v1.0.0", GitExtensions.GetTagList(workingDirectory: repository.Path));
        }
    }
}
