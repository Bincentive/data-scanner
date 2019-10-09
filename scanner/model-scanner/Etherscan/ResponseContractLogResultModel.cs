using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace model_scanner.Etherscan
{
    public class ResponseContractLogResultModel
    {
        /// <summary>
        /// address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; }
        /// <summary>
        /// topics
        /// </summary>
        [JsonPropertyName("topics")]
        public string[] Topics { get; set; }
        /// <summary>
        /// data
        /// </summary>
        [JsonPropertyName("data")]
        public string Data { get; set; }
        /// <summary>
        /// blockNumber
        /// </summary>
        [JsonPropertyName("blockNumber")]
        public string BlockNumber { get; set; }
        /// <summary>
        /// timeStamp
        /// </summary>
        [JsonPropertyName("timeStamp")]
        public string TimeStamp { get; set; }
        /// <summary>
        /// gasPrice
        /// </summary>
        [JsonPropertyName("gasPrice")]
        public string GasPrice { get; set; }
        /// <summary>
        /// gasUsed
        /// </summary>
        [JsonPropertyName("gasUsed")]
        public string GasUsed { get; set; }
        /// <summary>
        /// logIndex
        /// </summary>
        [JsonPropertyName("logIndex")]
        public string LogIndex { get; set; }
        /// <summary>
        /// transactionHash
        /// </summary>
        [JsonPropertyName("transactionHash")]
        public string TransactionHash { get; set; }
        /// <summary>
        /// transactionIndex
        /// </summary>
        [JsonPropertyName("transactionIndex")]
        public string TransactionIndex { get; set; }
    }
}
