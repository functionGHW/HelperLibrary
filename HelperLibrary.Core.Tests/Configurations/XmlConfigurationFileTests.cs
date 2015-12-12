/* 
 * FileName:    XmlConfigurationFileTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/18/2015 7:28:12 PM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Configurations.Tests
{
    using HelperLibrary.Core.Configurations;
    using HelperLibrary.Core.IOAbstractions;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    [TestFixture()]
    public class XmlConfigurationFileTests
    {
        #region Test Data
        private string[][] testCfgData = new[]
        {
            new[] {"version", "2.0"},
            new[] {"name", "TheName"},
            new[] {"id", "1234567"},
            new[] {"date", "2015/5/19"},
        };

        #endregion

        #region Tests for Property FilePath

        [Test]
        public void FilePathPropertyTest()
        {
            /* FilePath property read test
             */
            // arrange
            string filePath = @"test\test.xml";

            IFileSystem fileSystem = new Mock<IFileSystem>().Object;
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act
            string result = cfgFile.FilePath;

            // assert
            Assert.AreEqual(filePath, result);
        }
        #endregion

        #region Tests For Constructor

        [Test()]
        public void ConstructorNullParameterTest()
        {
            /* testing null parameter for constructor.
             */
            // arrange
            IFileSystem fileSystem = new Mock<IFileSystem>().Object;

            // act && assert
            Assert.Catch<ArgumentNullException>(() =>
                new XmlConfigurationFile("", fileSystem));

            Assert.Catch<ArgumentNullException>(() =>
                new XmlConfigurationFile(null, fileSystem));

            Assert.Catch<ArgumentNullException>(() =>
                new XmlConfigurationFile(@"test\test.xml", null));
        }

        #endregion

        #region Tests For ContainsConfiguration

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void ContainsConfigurationNullInputTest(string name)
        {
            /* testing null or empty string,
             * also testing strings only contains white spaces.
             * for these cases, the method should throw an exception.
             */
            // arrange
            string filePath = @"test\test.xml";
            IFileSystem fileSystem = new Mock<IFileSystem>().Object;
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act && assert
            Assert.Catch<ArgumentNullException>(
                () => cfgFile.ContainsConfiguration(name));
        }

        [TestCase("version", true)]
        [TestCase("id", true)]
        [TestCase("date", true)]
        [TestCase("name", true)]
        [TestCase("notsuchname", false)]
        public void ContainsConfigurationTest(string name, bool expected)
        {
            /* testing method ContainsConfiguration
             */
            // arrange
            string filePath = @"test\test.xml";

            IFileSystem fileSystem = BuildStubFileSystem();
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act
            bool result = cfgFile.ContainsConfiguration(name);

            // assert
            Assert.IsTrue(expected == result);
        }

        #endregion

        #region Tests For GetConfiguration

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetConfigurationNullInputTest(string name)
        {
            /* testing null or empty string,
             * also testing strings only contains white spaces.
             * for these cases, the method should throw an exception.
             */
            // arrange
            string filePath = @"test\test.xml";
            IFileSystem fileSystem = new Mock<IFileSystem>().Object;
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act && assert
            Assert.Catch<ArgumentNullException>(
                () => cfgFile.GetConfiguration(name));
        }

        [TestCaseSource(nameof(testCfgData))]
        [TestCase("notsuchname", null)]
        public void GetConfigurationTest(string name, string value)
        {
            /* testing method GetConfiguration
             */
            // arrange
            string filePath = @"test\test.xml";

            IFileSystem fileSystem = BuildStubFileSystem();
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act
            string result = cfgFile.GetConfiguration(name);

            // assert
            Assert.IsTrue(value == result);
        }

        #endregion

        #region Tests For AddConfiguration

        [TestCase(null, "testvalue")]
        [TestCase("", "testvalue")]
        [TestCase("  ", "testvalue")]
        [TestCase("testname", null)] // only test null, we allow to use empty string or white space as value
        public void AddConfigurationNullInputTest(string name, string value)
        {
            /* testing null or empty string,
             * also testing strings only contains white spaces.
             * for these cases, the method should throw an exception.
             */
            // arrange
            string filePath = @"test\test.xml";
            IFileSystem fileSystem = new Mock<IFileSystem>().Object;
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act && assert
            Assert.Catch<ArgumentNullException>(
                () => cfgFile.AddConfiguration(name, value));
        }

        [Test]
        public void AddConfigurationConfigurationAlreadyExistsTest()
        {
            /* testing when to add a configuration that already exists.
             * This should throw an exception
             */
            // arrange
            string name = "name"; // selected from Test Data
            string value = "anyvalue";
            string filePath = @"test\test.xml";

            IFileSystem fileSystem = BuildStubFileSystem(true);
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act && assert
            Assert.Catch<InvalidOperationException>(
                () => cfgFile.AddConfiguration(name, value));
        }

        [TestCase("testname", "testvalue")]
        [TestCase("testname", "")]
        [TestCase("testname", "  ")]
        public void AddConfigurationTest(string name, string value)
        {
            /* testing method AddConfiguration
             */
            // arrange
            string filePath = @"test\test.xml";

            IFileSystem fileSystem = BuildStubFileSystem(false);
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act
            string before = cfgFile.GetConfiguration(name);
            cfgFile.AddConfiguration(name, value);
            string after = cfgFile.GetConfiguration(name);

            // assert
            Assert.IsNull(before);
            Assert.AreEqual(value, after);
        }

        #endregion

        #region Tests For UpdateConfiguration

        [TestCase(null, "testvalue")]
        [TestCase("", "testvalue")]
        [TestCase("  ", "testvalue")]
        [TestCase("testname", null)] // only test null, we allow to use empty string or white space as value
        public void UpdateConfigurationNullInputTest(string name, string value)
        {
            /* testing null or empty string,
             * also testing strings only contains white spaces.
             * for these cases, the method should throw an exception.
             */
            // arrange
            string filePath = @"test\test.xml";
            IFileSystem fileSystem = new Mock<IFileSystem>().Object;
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act && assert
            Assert.Catch<ArgumentNullException>(
                () => cfgFile.UpdateConfiguration(name, value));
        }

        [Test]
        public void UpdateConfigurationConfigurationNotFoundTest()
        {
            /* testing when to add a configuration that already exists.
             * This should throw an exception
             */
            // arrange
            string name = "nosuchname"; // selected from Test Data
            string value = "anyvalue";
            string filePath = @"test\test.xml";

            IFileSystem fileSystem = BuildStubFileSystem(true);
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act && assert
            Assert.Catch<InvalidOperationException>(
                () => cfgFile.UpdateConfiguration(name, value));
        }

        [Test()]
        public void UpdateConfigurationTest()
        {
            /* testing method UpdateConfiguration
             */
            // arrange
            string name = testCfgData[0][0];
            string oldValue = testCfgData[0][1];
            string newValue = "newValue";
            string filePath = @"test\test.xml";

            IFileSystem fileSystem = BuildStubFileSystem();
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act
            string before = cfgFile.GetConfiguration(name);
            cfgFile.UpdateConfiguration(name, newValue);
            string after = cfgFile.GetConfiguration(name);

            // assert
            Assert.AreEqual(oldValue, before);
            Assert.AreEqual(newValue, after);
        }

        #endregion

        #region Tests For RemoveConfiguration

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void RemoveConfigurationNullInputTest(string name)
        {
            // arrange
            string filePath = @"test\Test.xml";

            IFileSystem fileSystem = BuildStubFileSystem();
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act && assert
            Assert.Catch<ArgumentNullException>(
                () => cfgFile.RemoveConfiguration(name));
        }

        [Test()]
        public void RemoveConfigurationTest()
        {
            /* testing method RemoveConfiguration
             */
            // arrange
            string name = "id";
            string filePath = @"test\test.xml";

            IFileSystem fileSystem = BuildStubFileSystem();
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act
            bool before = cfgFile.ContainsConfiguration(name);

            cfgFile.RemoveConfiguration(name);

            bool after = cfgFile.ContainsConfiguration(name);

            // assert
            Assert.IsTrue(before);
            Assert.IsFalse(after);
        }

        #endregion

        #region Tests For ToDictionary
        [Test()]
        public void ToDictionaryTest()
        {
            /* testing method ToDictionary
             */
            // arrange
            string filePath = @"test\test.xml";

            IFileSystem fileSystem = BuildStubFileSystem();
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act
            IDictionary<string, string> result = cfgFile.ToDictionary();

            // assert
            foreach (var item in testCfgData)
            {
                Assert.AreEqual(item[1], result[item[0]]);
            }
        }

        #endregion

        #region Tests For SaveChange

        [Test()]
        public void SaveChangeTest()
        {
            /* testing method SaveChange
             */
            // arrange
            string filePath = @"test\test.xml";
            string name = "name";
            string value = "value";
            int callCount = 0;

            var stubFileSystem = new Mock<IFileSystem>();
            stubFileSystem.Setup(f => f.FileExists(It.IsAny<string>()))
                .Returns(true);
            stubFileSystem.Setup(f =>
                f.Open(It.IsAny<string>(), It.IsAny<FileMode>(),
                    FileAccess.ReadWrite, FileShare.None))
                    .Returns(() => BuildTestFileStream(null))
                    .Callback(() => callCount++);

            IFileSystem fileSystem = stubFileSystem.Object;
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act
            cfgFile.AddConfiguration(name, value);

            bool isChangedBeforeAct = cfgFile.IsChanged;
            string getConfigurationBeforeAct = cfgFile.GetConfiguration(name);

            cfgFile.SaveChange();

            bool isChangedAfterAct = cfgFile.IsChanged;
            string getConfigurationAfterAct = cfgFile.GetConfiguration(name);

            // assert
            Assert.IsTrue(isChangedBeforeAct);
            Assert.IsFalse(isChangedAfterAct);

            Assert.AreEqual(value, getConfigurationBeforeAct);
            Assert.AreEqual(value, getConfigurationAfterAct);

            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region Tests For Reload

        [Test()]
        public void ReloadTest()
        {
            /* testing method Reload
             */
            // arrange
            string filePath = @"test\test.xml";
            string name = "name";
            string value = "value";
            int callCount = 0;

            var stubFileSystem = new Mock<IFileSystem>();
            stubFileSystem.Setup(f => f.FileExists(It.IsAny<string>()))
                .Returns(true);
            stubFileSystem.Setup(f =>
                f.Open(It.IsAny<string>(), It.IsAny<FileMode>(),
                    FileAccess.ReadWrite, FileShare.None))
                    .Returns(() => BuildTestFileStream(null))
                    .Callback(() => callCount++);

            IFileSystem fileSystem = stubFileSystem.Object;
            var cfgFile = new XmlConfigurationFile(filePath, fileSystem);

            // act
            cfgFile.AddConfiguration(name, value);

            bool isChangedBeforeAct = cfgFile.IsChanged;
            string getConfigurationBeforeAct = cfgFile.GetConfiguration(name);

            cfgFile.Reload();

            bool isChangedAfterAct = cfgFile.IsChanged;
            string getConfigurationAfterAct = cfgFile.GetConfiguration(name);

            // assert
            Assert.IsTrue(isChangedBeforeAct);
            Assert.IsFalse(isChangedAfterAct);

            Assert.AreEqual(value, getConfigurationBeforeAct);
            Assert.AreEqual(null, getConfigurationAfterAct);

            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region helper methods

        // return a memory stream to use as a File Stream
        private Stream BuildTestFileStream(string[][] cfgs)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            strBuilder.AppendLine("<configurations>");
            if (cfgs != null)
            {
                foreach (var item in cfgs)
                {
                    strBuilder.AppendFormat("<setting name=\"{0}\" value=\"{1}\" />", item[0], item[1]);
                }
            }
            strBuilder.AppendLine("</configurations>");

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strBuilder.ToString());
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(bytes, 0, bytes.Length);
            memoryStream.Position = 0;
            return memoryStream;
        }

        // helper method to build stub of IFileSystem
        private IFileSystem BuildStubFileSystem(bool fillData = true)
        {
            var stubFileSystem = new Mock<IFileSystem>();
            stubFileSystem.Setup(f => f.FileExists(It.IsAny<string>()))
                .Returns(true);

            stubFileSystem.Setup(f =>
                f.Open(It.IsAny<string>(), It.IsIn<FileMode>(FileMode.Open, FileMode.Create, FileMode.OpenOrCreate),
                    FileAccess.ReadWrite, FileShare.None))
                    .Returns(() => BuildTestFileStream(fillData ? this.testCfgData : null));

            IFileSystem fileSystem = stubFileSystem.Object;
            return fileSystem;
        }

        #endregion
    }
}
