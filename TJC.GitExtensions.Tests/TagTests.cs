using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class TagTests
    {
        [TestMethod]
        public void TagCommandsCreateAndListTags()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");

            GitExtensions.CreateTag("v1.0.0", repository.Path);

            CollectionAssert.Contains(
                GitExtensions.GetTagList(workingDirectory: repository.Path),
                "v1.0.0"
            );
        }
    }
}
