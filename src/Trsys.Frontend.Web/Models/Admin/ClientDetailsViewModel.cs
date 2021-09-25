﻿using NodaTime;
using System.Collections.Generic;
using Trsys.Frontend.Application.Admin.ClientDetails;
using Trsys.Frontend.Application.Dtos;

namespace Trsys.Frontend.Web.Models.Admin
{
    public class ClientDetailsViewModel
    {
        public ClientDetailsRequest Request { get; internal set; }
        public SecretKeyDto SecretKey { get; set; }
        public List<SubscriberTradeHistoryDto> TradeHistory { get; set; }
        public List<YearMonth> YearMonthSelection { get; internal set; }

    }
}
