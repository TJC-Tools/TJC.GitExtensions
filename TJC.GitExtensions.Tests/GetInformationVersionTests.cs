using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class GetInformationVersionTests
    {
        [TestMethod]
        public void GetInformation_ReturnsVersionAndDistanceForTaggedRepository()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            repository.Tag("v1.2.3");
            repository.Commit("second");

            var information = GitExtensions.GetInformation(repository.Path);

            Assert.IsTrue(information.IsVersionNumber);
            Assert.AreEqual(1, information.Major);
            Assert.AreEqual(2, information.Minor);
            Assert.AreEqual(3, information.Patch);
            Assert.AreEqual(1, information.DistanceToLatestTag);
            Assert.AreEqual(40, information.CommitHash.Length);
            Assert.IsFalse(information.IsDirty);
        }

        [TestMethod]
        public void GetInformation_ReturnsHashWithoutVersionWhenNoTagExists()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");

            var information = GitExtensions.GetInformation(repository.Path);

            Assert.IsFalse(information.IsVersionNumber);
            Assert.IsNull(information.Major);
            Assert.IsNull(information.Minor);
            Assert.IsNull(information.Patch);
            Assert.IsNull(information.DistanceToLatestTag);
            Assert.AreEqual(40, information.CommitHash.Length);
        }
    }
}
