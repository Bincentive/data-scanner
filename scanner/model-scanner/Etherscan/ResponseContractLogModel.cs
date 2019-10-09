using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace model_scanner.Etherscan
{
    /// <summary>
    /// ResponseContractLogModel
    /// </summary>
    public class ResponseContractLogModel
    {
        /// <summary>
        /// 狀態(1:OK)
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
        /// <summary>
        /// 訊息
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
        /// <summary>
        /// 回傳結果
        /// </summary>
        [JsonPropertyName("result")]
        public List<ResponseContractLogResultModel> Result { get; set; }
    }
}
