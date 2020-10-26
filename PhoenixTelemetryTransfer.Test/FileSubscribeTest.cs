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
    }
}
