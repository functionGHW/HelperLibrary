/* 
 * FileName:    DateTimeExtensionsTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  7/15/2015 6:54:07 PM
 * Version:     v1.0
 * Description:
 * */


namespace HelperLibrary.Core.ExtensionHelper.Tests
{
    using System;
    using HelperLibrary.Core.ExtensionHelper;
    using NUnit.Framework;

    [TestFixture()]
    public class DateTimeExtensionsTests
    {
        #region TestData

        private readonly object[] testData = new object[]
        {
            new object[]
            {
                new DateTime(2015, 7, 15, 12, 0, 0, DateTimeKind.Local),
                new DateTime(2015, 7, 15, 12, 0, 0, DateTimeKind.Local),
                true
            },
            new object[]
            {
                new DateTime(2016, 7, 15, 12, 0, 0, DateTimeKind.Local),
                new DateTime(2016, 7, 15, 12, 0, 1, DateTimeKind.Local),
                false
            },
            new object[]
            {
                new DateTime(2015, 8, 15, 12, 0, 0, DateTimeKind.Utc),
                new DateTime(2015, 8, 15, 12, 0, 0, DateTimeKind.Unspecified),
                true
            },
            new object[]
            {
                new DateTime(2015, 7, 14, 12, 0, 0, DateTimeKind.Local),
                new DateTime(2015, 7, 14, 12, 0, 0, DateTimeKind.Unspecified),
                false
            },
            new object[]
            {
                new DateTime(2015, 7, 15, 10, 0, 0, DateTimeKind.Local),
                new DateTime(2015, 7, 15, 10, 0, 0, DateTimeKind.Local).ToUniversalTime(),
                true
            },
            new object[]
            {
                new DateTime(2015, 7, 15, 10, 0, 0, DateTimeKind.Local),
                new DateTime(2015, 7, 15, 10, 0, 0, DateTimeKind.Utc),
                false
            },
        };

        #endregion

        #region Tests For EqualsWithTimeZone

        [TestCaseSource(nameof(testData))]
        public void EqualsWithTimeZoneTest(DateTime theDateTime, DateTime anotherDatetime, bool expected)
        {
            // arrange
            // act
            bool result = theDateTime.EqualsWithTimeZone(anotherDatetime);

            // assert
            Assert.IsTrue(expected == result);
        }

        #endregion
    }
}
