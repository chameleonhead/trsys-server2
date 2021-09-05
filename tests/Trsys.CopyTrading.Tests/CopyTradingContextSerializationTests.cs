using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Trsys.CopyTrading.Tests
{
    [TestClass]
    public class CopyTradingContextSerializationTests
    {
        [TestMethod]
        public void Serialize_ReturnXmlOfContext()
        {
            var expected = @"<?xml charset=""utf-8""?>
<CopyTradingContext>
</CopyTradingContext>
";

        }
    }
}