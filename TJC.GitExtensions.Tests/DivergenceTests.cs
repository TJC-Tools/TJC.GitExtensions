using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    public class DivergenceTests
    {
        [Fact]
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

            Assert.Equal(1, GitExtensions.GetAheadCount(repository.Path));
            Assert.Equal(1, GitExtensions.GetBehindCount(repository.Path));
        }
    }
}
