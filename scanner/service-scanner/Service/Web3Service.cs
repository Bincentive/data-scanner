using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using model_scanner.Common;
using model_scanner.Enum;
using model_scanner.Web3;
using model_scanner.Web3.Contract;
using service_scanner.Helper.Interface;
using service_scanner.Service.Interface;

namespace service_scanner.Service
{
    public class Web3Service:IWeb3Service
    {
        private readonly IWeb3Helper _web3Helper;
        public Web3Service(IWeb3Helper web3Helper)
        {
            this._web3Helper = web3Helper;
        }

        /// <summary>
        /// 取得TransactionHashEventLog
        /// </summary>
        /// <param name="transactionHash">The transaction hash.</param>
        /// <returns></returns>
        public async Task<List<ResponseTransactionHashEventLogModel>> GetTransactionHashEventLogAsync(string transactionHash)
        {
            var eventLogs = await this._web3Helper.GetTransactionReceiptForEventLogsAsync<EventEventDTO>(transactionHash);
            var result = new List<ResponseTransactionHashEventLogModel>();
            foreach (var item in eventLogs)
            {
                var eventLog = System.Text.Json.JsonSerializer.Deserialize<ResponseTransactionHashEventLogModel>(item.Event.tradingTx);
                result.Add(eventLog);
            }
            return result;
        }
    }
}
