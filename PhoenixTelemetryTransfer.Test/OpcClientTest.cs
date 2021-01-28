using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace PhoenixTelemetryTransfer.Test
{
    [TestFixture]
    class OpcClientTest
    {
        [Test]
        public void WriteGroupData_EmptyList_ThrowsNoException()
        {
            var OpcClientTest = new OpcClient("testurl");
            OpcClientTest.WriteGroupData();

            Assert.That(() => OpcClientTest.WriteGroupData(), Throws.Nothing);
        }

        [Test]
        [TestCase(null, 1.2)]
        [TestCase("", 1.2)]
        [TestCase("tag1",null)]
        [TestCase("tag1", 1.2)]
        [TestCase(null, null)]
        public void AddTagValue_EmptyStringAndValue_ThrowsNoException(string name, double value)
        {

            var OpcClientTest = new OpcClient("testurl");
            OpcClientTest.AddTagValue(name, value);
            Assert.That(() => OpcClientTest.AddTagValue(name, value), Throws.Nothing);
        }
    }
}
