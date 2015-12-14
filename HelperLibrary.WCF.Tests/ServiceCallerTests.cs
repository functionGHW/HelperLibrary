/* 
 * FileName:    ServiceCallerTests.cs
 * Author:      functionghw<functionghw@hotmail.com>
 * CreateTime:  12/14/2015 3:43:20 PM
 * Version:     v1.0
 * Description:
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperLibrary.WCF.Proxy;
using Moq;
using NUnit.Framework;

namespace HelperLibrary.WCF.Tests
{
    [TestFixture()]
    public class ServiceCallerTests
    {
        #region Test Data

        public interface ITestService
        {
            void Foo();

            int Bar();
        }

        #endregion

        #region Tests For Constructor
        [Test]
        public void ConstructorNullParameterTest()
        {
            Assert.Catch<ArgumentNullException>(
                () => new ServiceCaller(null));
        }

        #endregion

        #region Tests for CallService

        [Test]
        public void CallServiceNullParameterTest()
        {
            // arrange
            var proxyBuilderStub = new Mock<IProxyBuilder>();
            var proxyBuilder = proxyBuilderStub.Object;
            var caller = new ServiceCaller(proxyBuilder);

            // act & assert
            Assert.Catch<ArgumentNullException>(
                () => caller.CallService<ITestService>(null));

            Assert.Catch<ArgumentNullException>(
                () => caller.CallService<ITestService>(null, () => { }));

            Assert.Catch<ArgumentNullException>(
                () => caller.CallService<ITestService, string>(null));

            Assert.Catch<ArgumentNullException>(
                () => caller.CallService<ITestService, string>(null, arg => { }));

        }

        [Test]
        public void CallServiceNoReturnTest()
        {
            // arrange
            int actionInvokeCount = 0;
            int callbackInvokeCount = 0;

            var serviceStub = new Mock<ITestService>();
            serviceStub.Setup(s => s.Foo())
                .Callback(() => actionInvokeCount++);
            var serviceObj = serviceStub.Object;

            var proxyBuilderStub = new Mock<IProxyBuilder>();
            proxyBuilderStub.Setup(builder => builder.GetProxy<ITestService>())
                .Returns(serviceObj);

            var proxyBuilder = proxyBuilderStub.Object;
            var caller = new ServiceCaller(proxyBuilder);

            // act & assert
            caller.CallService<ITestService>(service => service.Foo());
            Assert.AreEqual(1, actionInvokeCount);

            // act & assert
            caller.CallService<ITestService>(service => service.Foo(), () => callbackInvokeCount++);
            Assert.AreEqual(2, actionInvokeCount);
            Assert.AreEqual(1, callbackInvokeCount);
        }

        [Test]
        public void CallServiceWithReturnValueTest()
        {
            // arrange
            int actionInvokeCount = 0;
            int callbackInvokeCount = 0;

            var serviceStub = new Mock<ITestService>();
            serviceStub.Setup(s => s.Bar())
                .Returns(() => ++actionInvokeCount);
            var serviceObj = serviceStub.Object;

            var proxyBuilderStub = new Mock<IProxyBuilder>();
            proxyBuilderStub.Setup(builder => builder.GetProxy<ITestService>())
                .Returns(serviceObj);

            var proxyBuilder = proxyBuilderStub.Object;
            var caller = new ServiceCaller(proxyBuilder);

            // act & assert
            int result = caller.CallService<ITestService, int>(service => service.Bar());
            Assert.AreEqual(1, result);

            // act & assert
            result = caller.CallService<ITestService, int>(service => service.Bar(), val => callbackInvokeCount++);
            Assert.AreEqual(2, result);
            Assert.AreEqual(1, callbackInvokeCount);
        }

        #endregion

    }
}
