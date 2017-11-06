using System.IO;
using NUnit.Framework;

namespace UserStorageServices.Tests
{
    using static UserTestHealper;

    class UserDiskRepository
    {
        private const string RepositoryFileName = "test_repository.bin";

        [TearDown]
        public void DeleteRepositoryFile()
        {
            if (File.Exists(RepositoryFileName))
            {
                File.Delete(RepositoryFileName);
            }
        }

        [Test]
        public void StartAfterStop_ResultBeforStopIsEqualToResultAfterStart()
        {
            // Arrange
            var userMemoryCacheWithStateStop = new Repositories.UserDiskRepository(RepositoryFileName);
            InitUserRepository(userMemoryCacheWithStateStop);

            var userMemoryCacheWithStateStart = new Repositories.UserDiskRepository(RepositoryFileName);

            // Act
            userMemoryCacheWithStateStop.Stop();
            userMemoryCacheWithStateStart.Start();

            // Assert
            AssertAreEqual(
                userMemoryCacheWithStateStop.Query((u) => true),
                userMemoryCacheWithStateStart.Query((u) => true),
                CompareUsers);
        }
    }
}
