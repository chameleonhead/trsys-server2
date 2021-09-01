using System;
using System.Collections.Generic;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogParser
    {
        public static IEnumerable<IEvent> Parse(string text)
        {
            return Array.Empty<IEvent>();
        }
    }
}
