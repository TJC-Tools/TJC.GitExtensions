using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    [TestClass]
    public class CloneTests
    {
        [TestMethod]
        public void CloneCopiesRepository()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            var clonePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            try
            {
                GitExtensions.Clone(repository.Path, clonePath);

                Assert.IsTrue(File.Exists(Path.Combine(clonePath, "file.txt")));
                Assert.AreEqual("initial", File.ReadAllText(Path.Combine(clonePath, "file.txt")));
            }
            finally
            {
                if (Directory.Exists(clonePath))
                {
                    foreach (var file in Directory.EnumerateFiles(clonePath, "*", SearchOption.AllDirectories))
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                    }

                    Directory.Delete(clonePath, recursive: true);
                }
            }
        }
    }
}