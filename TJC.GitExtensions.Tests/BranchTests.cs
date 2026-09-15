using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    
    public class BranchTests
    {
        [Fact]
        public void BranchCommandsReadAndChangeCurrentBranch()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            var initialBranch = GitExtensions.GetBranchName(repository.Path);
            repository.CreateBranch("work");

            GitExtensions.ChangeBranch("work", repository.Path);
            Assert.Equal("work", GitExtensions.GetBranchName(repository.Path));

            var branches = GitExtensions.GetBranchList(repository.Path);
            Assert.True(branches.Any(branch => branch.Name == "work" && branch.IsCurrent));
            Assert.True(
                branches.Any(branch => branch.Name == initialBranch && !branch.IsCurrent)
            );

            GitExtensions.Checkout(initialBranch, repository.Path);
            Assert.Equal(initialBranch, GitExtensions.GetBranchName(repository.Path));
        }
    }
}
