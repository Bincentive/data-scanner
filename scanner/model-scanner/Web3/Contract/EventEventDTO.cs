using System;
using System.Collections.Generic;
using System.Text;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace model_scanner.Web3.Contract
{
    /// <summary>
    /// 此地址裡面用的合約地址ABI裡面Event
    /// </summary>
    /// <seealso cref="Nethereum.ABI.FunctionEncoding.Attributes.IEventDTO" />
    [Event("LogTraderTradingTransaction")]
    public class EventEventDTO : IEventDTO
    {
        [Parameter("string", "tradingTx", 1, false)]
        public string tradingTx { get; set; }
    }
}
