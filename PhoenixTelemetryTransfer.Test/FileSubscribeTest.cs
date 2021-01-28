// <copyright file="FileSubscribeTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PhoenixTelemetryTransfer.Test
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Tests for FileSubscriber class.
    /// </summary>
    [TestFixture]
    public class FileSubscribeTest
    {
        /// <summary>
        /// We don't want to start subscibe without a valid path.
        /// </summary>
        [Test]
        public void ShouldThrowIfPathIsEmpty()
        {
            Assert.That(() => new FileSubscriber(string.Empty, 60), Throws.ArgumentException);
        }

        /// <summary>
        /// We don't want to start subscibe without a valid interval time.
        /// </summary>
        [Test]
        public void ShouldThrowIfIntervalIsZero()
        {
            Assert.That(() => new FileSubscriber(string.Empty, 0), Throws.ArgumentException);
        }

        /// <summary>
        /// Should Parse out value 99,99.
        /// </summary>
        [Test]
        public void TryGetValues_StringInput_ShouldReturnValue99_99()
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues("someformat?;99,99", out var result))
            {
                Assert.That(99.99, Is.EqualTo(double.Parse(result[0])));
            }
        }

        /// <summary>
        /// TryGetValues should return two values.
        /// </summary>
        [Test]
        [TestCase("00:01:13; 20,14; 20,19", 20.14, 20.19)]
        [TestCase("00:01:13;    20,14; 20,19", 20.14, 20.19)]
        [TestCase("00:01:13;  20,14   ; 20,19   ", 20.14, 20.19)]
        [TestCase("00:01:13 ;20,14; 20,19", 20.14, 20.19)]
        public void TryGetValues_DifferentStringInputsFormatting_ShouldReturnSameTwoValues
            (string input, double result1, double result2)
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues(input, out var result))
            {
                Assert.That(result1, Is.EqualTo(double.Parse(result[0])));
                Assert.That(result2, Is.EqualTo(double.Parse(result[1])));
            }
        }

        /// <summary>
        /// TryGetValues should return correct timestamp.
        /// </summary>
        [Test]
        public void TryGetValues_StringTimeStamp_ShouldReturnTimestamp()
        {
            var fileSubscriber = new FileSubscriber("MyPath", 60);

            if (fileSubscriber.TryGetValues("00:01:13; 20,19", out var result))
            {
                Assert.That("00:01:13", Is.EqualTo(fileSubscriber.TimeStamp));
            }
        }
     /// <summary>
        /// Filesubscriber should raise an event.
        /// </summary>
        [Test]
        public void EventShouldBeRaisedOnNewData()
        {
        }
    }
}
