using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace model_scanner.Web3
{
    /// <summary>
    /// ResponseTransactionHashEventLogModel
    /// </summary>
    public class ResponseTransactionHashEventLogModel
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("Symbol")]
        public string Symbol { get; set; }
        /// <summary>
        /// OrderID
        /// </summary>
        [JsonPropertyName("OrderID")]
        public string OrderID { get; set; }
        /// <summary>
        /// ExecID
        /// </summary>
        [JsonPropertyName("ExecID")]
        public string ExecID { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        [JsonPropertyName("Price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Qty
        /// </summary>
        [JsonPropertyName("Qty")]
        public decimal Qty { get; set; }
        /// <summary>
        /// Side
        /// </summary>
        [JsonPropertyName("Side")]
        public string Side { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("Timestamp")]
        public string Timestamp { get; set; }

    }
}
