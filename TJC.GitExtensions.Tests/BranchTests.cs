using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class BranchTests
    {
        [TestMethod]
        public void BranchCommandsReadAndChangeCurrentBranch()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            var initialBranch = GitExtensions.GetBranchName(repository.Path);
            repository.CreateBranch("work");

            GitExtensions.ChangeBranch("work", repository.Path);
            Assert.AreEqual("work", GitExtensions.GetBranchName(repository.Path));

            GitExtensions.Checkout(initialBranch, repository.Path);
            Assert.AreEqual(initialBranch, GitExtensions.GetBranchName(repository.Path));
        }
    }
}
