using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    public class GetInformationVersionTests
    {
        [Fact]
        public void GetInformation_ReturnsVersionAndDistanceForTaggedRepository()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            repository.Tag("v1.2.3");
            repository.Commit("second");

            var information = GitExtensions.GetInformation(repository.Path);

            Assert.True(information.IsVersionNumber);
            Assert.Equal(1, information.Major);
            Assert.Equal(2, information.Minor);
            Assert.Equal(3, information.Patch);
            Assert.Equal(1, information.DistanceToLatestTag);
            Assert.Equal(40, information.CommitHash.Length);
            Assert.False(information.IsDirty);
        }

        [Fact]
        public void GetInformation_ReturnsHashWithoutVersionWhenNoTagExists()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");

            var information = GitExtensions.GetInformation(repository.Path);

            Assert.False(information.IsVersionNumber);
            Assert.Null(information.Major);
            Assert.Null(information.Minor);
            Assert.Null(information.Patch);
            Assert.Null(information.DistanceToLatestTag);
            Assert.Equal(40, information.CommitHash.Length);
        }
    }
}
