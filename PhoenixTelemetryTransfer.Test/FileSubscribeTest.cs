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
                Assert.AreEqual(99.99f, double.Parse(result[0]), 0.1f);
            }

        }

        [TestMethod]
        public void HandleBlankspace_return20_14()
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues("00:01:12;    20,14", out var result))
            {
                Assert.AreEqual(20.14f, double.Parse(result[0]), 0.1f);
            }

        }

        [TestMethod]
        public void ShouldReturnTwoValues()
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues("00:01:13;    20,14;   20,19", out var result))
            {
                Assert.AreEqual(20.14f, double.Parse(result[0]), 0.1f);
                Assert.AreEqual(20.19f, double.Parse(result[1]), 0.1f);
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

            if (fileSubscriber.TryGetValues("00:01:13;  20,19;   21,534;     22,54;  19,51;  24,89;  19,0;   24,65;   20;    21,15;  24,501;    11,96;" +
                "   23,15;     22,11;  23,65;  15,95;  21,546;     17,62;  21,77;  19,01;  20,02", out var result))
            {
                Assert.AreEqual(20.19f, double.Parse(result[0]), 0.1f);
                Assert.AreEqual(21.534f, double.Parse(result[1]), 0.1f);
                Assert.AreEqual(24.501f, double.Parse(result[9]), 0.1f);
                Assert.AreEqual(11.96f, double.Parse(result[10]), 0.1f);
                Assert.AreEqual(19.01f, double.Parse(result[18]), 0.1f);
                Assert.AreEqual(20.02f, double.Parse(result[19]), 0.1f); //20e och sista channel value
            }

        }

    }
}
