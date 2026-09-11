using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class DivergenceTests
    {
        [TestMethod]
        public void GetDivergenceCountsReturnsAheadAndBehindCommits()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            repository.CreateBranch("base");
            repository.CreateBranch("work");
            repository.Checkout("work");
            repository.Commit("ahead");
            repository.Checkout("base");
            repository.Commit("behind");
            repository.Checkout("work");
            repository.SetUpstream("base");

            Assert.AreEqual(1, GitExtensions.GetAheadCount(repository.Path));
            Assert.AreEqual(1, GitExtensions.GetBehindCount(repository.Path));
        }
    }
}
