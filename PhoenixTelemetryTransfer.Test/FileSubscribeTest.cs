namespace PhoenixTelemetryTransfer.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FileSubscribeTest
    {
        /// <summary>
        /// We don't want to start subscibe without a valid path.
        /// </summary>
        [TestMethod]
        public void ShouldThrowIfPathIsEmpty()
        {
            Assert.ThrowsException<ArgumentException>(() => new FileSubscriber(string.Empty, 60));
        }

        /// <summary>
        /// We don't want to start subscibe without a valid interval time.
        /// </summary>
        [TestMethod]
        public void ShouldThrowIfIntervalIsZero()
        {
            Assert.ThrowsException<ArgumentException>(() => new FileSubscriber(string.Empty, 0));
        }

        /// <summary>
        /// Should Parse out value 99,99.
        /// </summary>
        [TestMethod]
        public void ShouldReturnValue99_99()
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues("someformat?;99,99", out var result))
            {
                Assert.AreEqual(99.99, double.Parse(result[0]));
            }

        }

        [TestMethod]
        public void HandleBlankspace_return20_14()
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues("00:01:12;    20,14", out var result))
            {
                Assert.AreEqual(20.14, double.Parse(result[0]));
            }
        }

        [TestMethod]
        public void ShouldReturnTwoValues()
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues("00:01:13;    20,14;   20,19", out var result))
            {
                Assert.AreEqual(20.14, double.Parse(result[0]));
                Assert.AreEqual(20.19, double.Parse(result[1]));
            }

        }

        [TestMethod]
        public void shouldReturnTimestamp_00_01_13()
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues("00:01:13; 20,19", out var result))
            {
                Assert.AreEqual("00:01:13", fileSubscriber.TimeStamp);
            }

        }

        [TestMethod]
        public void shouldReturnMaxNrOfValues()
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues(
                "00:01:13;  20,19;   21,534;     22,54;  19,51;  24,89;  19,0;   24,65;   20;    21,15;  24,501;    11,96;" +
                "   23,15;     22,11;  23,65;  15,95;  21,546;     17,62;  21,77;  19,01;  20,02", out var result))
            {
                Assert.AreEqual(20.19, double.Parse(result[0]));
                Assert.AreEqual(21.534, double.Parse(result[1]));
                Assert.AreEqual(22.54, double.Parse(result[2]));
                Assert.AreEqual(19.51, double.Parse(result[3]));
                Assert.AreEqual(24.89, double.Parse(result[4]));
                Assert.AreEqual(19.0, double.Parse(result[5]));
                Assert.AreEqual(24.65, double.Parse(result[6]));
                Assert.AreEqual(20, double.Parse(result[7]));
                Assert.AreEqual(21.15, double.Parse(result[8]));
                Assert.AreEqual(24.501, double.Parse(result[9]));
                Assert.AreEqual(11.96, double.Parse(result[10]));
                Assert.AreEqual(23.15, double.Parse(result[11]));
                Assert.AreEqual(22.11, double.Parse(result[12]));
                Assert.AreEqual(23.65, double.Parse(result[13]));
                Assert.AreEqual(15.95, double.Parse(result[14]));
                Assert.AreEqual(21.546, double.Parse(result[15]));
                Assert.AreEqual(17.62, double.Parse(result[16]));
                Assert.AreEqual(21.77, double.Parse(result[17]));
                Assert.AreEqual(19.01, double.Parse(result[18]));
                Assert.AreEqual(20.02, double.Parse(result[19]));
            }
        }
    }
}
