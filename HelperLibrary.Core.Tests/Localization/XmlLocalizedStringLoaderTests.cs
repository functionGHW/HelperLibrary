using HelperLibrary.Core.IOAbstractions;
/* 
 * FileName:    XmlLocalizedStringLoaderTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/18/2015 11:05:17 AM
 * Version:     v1.0
 * Description:
 * */

namespace HelperLibrary.Core.Localization.Tests
{
    using HelperLibrary.Core.Localization;
    using NUnit.Framework;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.IO;

    [TestFixture()]
    public class XmlLocalizedStringLoaderTests
    {
        #region Test Datas

        private string testXmlData = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                        "<LocalizationCollection CultureName=\"en-US\">" +
                        "    <StringItem>" +
                        "        <MsgId>test</MsgId>" +
                        "        <MsgStr>Test</MsgStr>" +
                        "    </StringItem>" +
                        "</LocalizationCollection>";

        #endregion

        #region Tests For Constructor

        [Test()]
        public void ConstructorNullParameterTest()
        {
            /* null test for constructor of XmlLocalizedStringLoader
             */
            // arrange
            IFileSystem fileSystem = null;

            // act && assert
            Assert.Catch<ArgumentNullException>(() => new XmlLocalizedStringLoader(fileSystem));
        }

        #endregion

        #region Tests For GetLocalizedDictionary

        [TestCase("scope", "")]
        [TestCase("scope", null)]
        [TestCase("", "en-US")]
        [TestCase(null, "en-US")]
        public void GetLocalizedDictionaryNullParameterTest(string scope, string cultureName)
        {
            /* null parameters tests
             */
            // arrange
            var stubFileSystem = new Mock<IFileSystem>();

            IFileSystem fileSystem = stubFileSystem.Object;
            XmlLocalizedStringLoader loader = new XmlLocalizedStringLoader(fileSystem);

            // act && assert
            Assert.Catch<ArgumentNullException>(() =>
                loader.GetLocalizedDictionary(scope, cultureName));
        }

        [Test()]
        public void GetLocalizedDictionaryTest()
        {
            /* check if the method can read the xml file.
             */
            // arrange
            string scope = "scope";
            string cultureName = "en-US";

            string filePath = string.Format(@"Localization\{1}\{0}.{1}.xml", scope, cultureName);

            var stubFileSystem = new Mock<IFileSystem>();
            stubFileSystem.Setup(f => f.FileExists(It.IsAny<string>()))
                .Returns(true);
            stubFileSystem.Setup(f => f.OpenRead(filePath))
                .Returns(() => GetFakeXmlFileStream());

            IFileSystem fileSystem = stubFileSystem.Object;
            XmlLocalizedStringLoader loader = new XmlLocalizedStringLoader(fileSystem);

            // act
            IDictionary<string, string> result = loader.GetLocalizedDictionary(scope, cultureName);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result["test"] == "Test");
        }

        #endregion

        #region Helper methods

        private Stream GetFakeXmlFileStream()
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(testXmlData);
            MemoryStream memoryStream = new MemoryStream(bytes);
            return memoryStream;
        }

        #endregion
    }
}
