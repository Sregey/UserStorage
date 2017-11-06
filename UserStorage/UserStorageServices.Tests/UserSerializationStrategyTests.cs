using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace UserStorageServices.Tests
{
    class UserSerializationStrategyTests
    {
        private string expectedXmlSerializePath = "test_xml_repository.bin";
        private string expectedBinSerializePath = "test_bin_repository.bin";
        private string actualSerializePath = "test_result.bin";

        public IEnumerable<TestCaseData> Serializers
        {
            get
            {
                yield return new TestCaseData(
                    new BinaryUserSerializationStrategy(), 
                    expectedBinSerializePath);

                yield return new TestCaseData(
                    new XmlUserSerializationStrategy(), 
                    expectedXmlSerializePath);
            }
        }

        [TearDown]
        public void DeleteRepositoryFile()
        {
            if (File.Exists(actualSerializePath))
            {
                File.Delete(actualSerializePath);
            }
        }

        [Test, TestCaseSource(nameof(Serializers))]
        public void Serialize_ValidArguments_CreateFileWithSpecialFormat(
            IUserSerializationStrategy userSerializer, 
            string expectedPath)
        {
            // Arrange
            var users = UserTestHealper.Users;

            // Act
            userSerializer.SerializeUsers(actualSerializePath, users);

            // Assert
            FileAssert.AreEqual(expectedPath, actualSerializePath);
        }

        [Test, TestCaseSource(nameof(Serializers))]
        public void Deserialize_ValidArguments_ReturnListOfUsers(IUserSerializationStrategy userSerializer, string expectedPath)
        {
            // Arrange
            var users = UserTestHealper.Users;

            // Act
            var actualUsers = userSerializer.DeserializeUsers(expectedPath);

            // Assert
            UserTestHealper.AssertAreEqual(users, actualUsers, UserTestHealper.CompareUsers);
        }
    }
}
