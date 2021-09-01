using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trsys.CopyTrading.Abstractions;

namespace Trsys.CopyTrading.Tests
{
    [TestClass]
    public class EaLogParserTest
    {
        [TestMethod]
        public void InitLog_ShouldParsed()
        {
            var events = EaLogParser
                .Parse(
                    DateTimeOffset.Parse("2021-08-20T16:55:54.950Z"),
                    "KEY",
                    "Publisher",
                    "TOKEN",
                    "VERSION",
                    "1629456515:DEBUG:Init\r\n1629456515:DEBUG:Local order opened. LocalOrder = 0/27636811/GBPJPY.oj1m/1\r\n1629456688:DEBUG:Local order closed. LocalOrder = 0/27636811/GBPJPY.oj1m/1\r\n1629456952:DEBUG:Deinit. Reason = 3")
                .ToArray();
            Assert.AreEqual(4, events.Count());
            Assert.AreEqual("EaLogInit", events[0].Type);
            Assert.AreEqual("EaLogLocalOrderOpened", events[1].Type);
            Assert.AreEqual("EaLogLocalOrderClosed", events[2].Type);
            Assert.AreEqual("EaLogDeinit", events[3].Type);
        }
    }
}
