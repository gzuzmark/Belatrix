using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using Moq;

namespace Belatrix.JobLogger.Tests
{
    [TestFixture]
    public class LoggerTests
    {
        private Mock<IJobLogger> _textFileJobLoggerMock;
        private Mock<IJobLogger> _consoleJobLoggerMock;
        private Mock<IJobLogger> _databaseJobLoggerMock;

        [Test]
        public void WhenThereAreNotLogTypes()
        {
            Logger.SetLogTypes(LogType.Warning, LogType.Info);
            var message = "Error 500 Internal Server Error";
            var result = false;
            
            result = Logger.LogMessage(message, LogType.Error);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void WhenLogTypesAreSetAndSourceIsFile()
        {
            Logger.SetLogTypes(LogType.Info, LogType.Warning, LogType.Error);
            Logger.SetLogSources(LogSource.File);
            var message = "Random error message";
            var expectedResult = true;
            SetUptJobLoggersMock();

            var result = Logger.LogMessage(message, LogType.Error);

            _textFileJobLoggerMock.Verify(m => m.LogMessage(message, LogType.Error), Times.Once());
            _consoleJobLoggerMock.Verify(m => m.LogMessage(message, LogType.Error), Times.Never());
            _databaseJobLoggerMock.Verify(m => m.LogMessage(message, LogType.Error), Times.Never());
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void WhenAllLogTypesAndConsoleSource()
        {
            Logger.SetLogTypes(LogType.Info, LogType.Warning, LogType.Error);
            Logger.SetLogSources(LogSource.Console);
            var message = "Random error message";
            var expectedResult = true;
            SetUptJobLoggersMock();

            var result = Logger.LogMessage(message, LogType.Error);

            _consoleJobLoggerMock.Verify(m => m.LogMessage(message, LogType.Error), Times.Once());
            _textFileJobLoggerMock.Verify(m => m.LogMessage(message, LogType.Error), Times.Never());
            _databaseJobLoggerMock.Verify(m => m.LogMessage(message, LogType.Error), Times.Never());
            Assert.AreEqual(expectedResult, result);
        }

       

        [Test]
        public void WhenAllLogTypesAndAllLogSources()
        {
            Logger.SetLogTypes(LogType.Info, LogType.Warning, LogType.Error);
            Logger.SetLogSources(LogSource.Database, LogSource.Console, LogSource.File);
            var message = "Random error message";
            var expectedResult = true;
            SetUptJobLoggersMock();

            var result = Logger.LogMessage(message, LogType.Error);

            _consoleJobLoggerMock.Verify(m => m.LogMessage(message, LogType.Error), Times.Once());
            _textFileJobLoggerMock.Verify(m => m.LogMessage(message, LogType.Error), Times.Once());
            _databaseJobLoggerMock.Verify(m => m.LogMessage(message, LogType.Error), Times.Once());
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void WhenLogTypesAreEmpty()
        {
            Logger.SetLogTypes();
            Logger.SetLogSources(LogSource.Database, LogSource.Console, LogSource.File);
            var message = "Random error message";
            
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                Logger.LogMessage(message, LogType.Error);
            });
        }

        [Test]
        public void WhenErrorSourcesAreEmpty()
        {
            Logger.SetLogTypes(LogType.Error, LogType.Warning, LogType.Info);
            Logger.SetLogSources();
            var message = "Random error message";

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                Logger.LogMessage(message, LogType.Error);
            });
        }

        private void SetUptJobLoggersMock()
        {
            _textFileJobLoggerMock = new Mock<IJobLogger>();
            _consoleJobLoggerMock = new Mock<IJobLogger>();
            _databaseJobLoggerMock = new Mock<IJobLogger>();

            Logger.SetUpJobLoggers(_textFileJobLoggerMock.Object, _consoleJobLoggerMock.Object, _databaseJobLoggerMock.Object);
        }
    }
}
