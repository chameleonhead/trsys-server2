using Akka.Actor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Trsys.CopyTrading.Domain.Tests
{
    [TestClass]
    public class CopyTradingAggregateTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Create actor system
            var system = ActorSystem.Create("copy-trading");

            //Create supervising aggregate manager for UserAccount aggregate root actors
            var aggregateManager = system.ActorOf(Props.Create(() => new CopyTradeAggregateManager()));

            //Build create user account aggregate command with name "foo bar"
            var aggregateId = CopyTradeId.New;
            var openCommand = new OpenCopyTradeCommand(aggregateId, "USDJPY", "BUY");

            //Send the command to create the aggregate
            aggregateManager.Tell(openCommand);

            var closeCommand = new CloseCopyTradeCommand(aggregateId);
            aggregateManager.Tell(closeCommand);

            Thread.Sleep(200);
        }
    }
}
