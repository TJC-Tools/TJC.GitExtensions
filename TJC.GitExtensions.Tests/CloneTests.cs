using TJC.GitExtensions;

namespace TJC.GitExtensions.Tests
{
    
    public class CloneTests
    {
        [Fact]
        public void CloneCopiesRepository()
        {
            using var repository = TestRepository.Create();
            repository.Commit("initial");
            var clonePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            try
            {
                GitExtensions.Clone(repository.Path, clonePath);

                Assert.True(File.Exists(Path.Combine(clonePath, "file.txt")));
                Assert.Equal("initial", File.ReadAllText(Path.Combine(clonePath, "file.txt")));
            }
            finally
            {
                if (Directory.Exists(clonePath))
                {
                    foreach (
                        var file in Directory.EnumerateFiles(
                            clonePath,
                            "*",
                            SearchOption.AllDirectories
                        )
                    )
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                    }

                    Directory.Delete(clonePath, recursive: true);
                }
            }
        }
    }
}
