﻿using System;

namespace Trsys.CopyTrading.Abstractions
{
    public class EaLogLine
    {
        public DateTimeOffset? Timestamp { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
    }
}