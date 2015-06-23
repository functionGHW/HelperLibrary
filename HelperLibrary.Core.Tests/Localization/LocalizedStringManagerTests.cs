/* 
 * FileName:    LocalizedStringManagerTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  5/15/2015 2:11:10 PM
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

    [TestFixture()]
    public class LocalizedStringManagerTests
    {
        #region Test Datas

        private IDictionary<string, string> englishWordsTestData = new Dictionary<string, string>()
        {
            {"hello", "Hello!"},
            {"RequireInput", "Please input something!"},
        };

        #endregion

        #region Tests For Property Default

        [Test()]
        public void DefaultPropertyTest()
        {
            // act
            var result = LocalizedStringManager.Default;

            // assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Tests For Constructor

        [Test]
        public void ConstructorNullInputTest()
        {
            // arrange
            ILocalizedStringLoader loader = null;

            // act && assert
            Assert.Catch<ArgumentNullException>(() => new LocalizedStringManager(loader));
        }

        #endregion

        #region Tests For GetLocalizedString

        [TestCase("scope", "")]
        [TestCase("scope", null)]
        [TestCase("", "key")]
        [TestCase(null, "key")]
        public void GetLocalizedStringNullOrEmptyInputTest(string scope, string key)
        {
            /* exception testing of parameter, it should throw exceptions 
             * when scope and/or key is null or empty string.
             */
            // arrange
            string cultureName = "en-US";
            LocalizedStringManager strMgr = BuildFakeStrMgr(scope, cultureName);

            // act && assert
            Assert.Catch<ArgumentNullException>(() =>
                strMgr.GetLocalizedString(scope, key, cultureName));

        }

        [TestCase("hello", "Hello!")]
        [TestCase("RequireInput", "Please input something!")]
        [TestCase("keynotfound", "keynotfound")] // this key should not exist in the test dict.
        public void GetLocalizedStringTest(string key, string value)
        {
            /* testing if the method works well. 
             * if key not exist, it will simply return key.
             * otherwise return the vlue.
             */
            // arrange
            string scope = "scope";
            string cultureName = "en-US";

            LocalizedStringManager strMgr = BuildFakeStrMgr(scope, cultureName);

            // act
            string result = strMgr.GetLocalizedString(scope, key, cultureName);

            // assert
            Assert.IsTrue(result == value);
        }

        #endregion

        #region Tests For ReloadLocalizedStrings

        [Test()]
        public void ReloadLocalizedStringsTest()
        {
            /* testing ReloadLocalizedStrings method.
             * this method will clear cache of the LocalizedStringManager,
             * when query the same key after calling this method,
             * the GetLocalizedDictionary should be called again.
             * Thus the callCount should be 2.
             */
            // arrange
            string scope = "scope";
            string cultureName = "en-US";
            string key = "anykey";
            int callCount = 0;

            var stubLoader = new Mock<ILocalizedStringLoader>();
            stubLoader.Setup(l => l.GetLocalizedDictionary(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new Dictionary<string, string>())
                .Callback(() => callCount++);

            ILocalizedStringLoader loader = stubLoader.Object;
            LocalizedStringManager strMgr = new LocalizedStringManager(loader);

            // act
            strMgr.GetLocalizedString(scope, key, cultureName);
            strMgr.ReloadLocalizedStrings();
            strMgr.GetLocalizedString(scope, key, cultureName);

            // assert
            Assert.IsTrue(callCount == 2);
        }

        #endregion

        #region helper methods

        private LocalizedStringManager BuildFakeStrMgr(string scope, string cultureName)
        {
            var stubLoader = new Mock<ILocalizedStringLoader>();
            stubLoader.Setup(l => l.GetLocalizedDictionary(scope, cultureName))
                .Returns(englishWordsTestData);

            ILocalizedStringLoader loader = stubLoader.Object;
            LocalizedStringManager strMgr = new LocalizedStringManager(loader);

            return strMgr;
        }

        #endregion
    }
}
