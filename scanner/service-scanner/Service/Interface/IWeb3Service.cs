using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using model_scanner.Web3;

namespace service_scanner.Service.Interface
{
    /// <summary>
    /// IWeb3Service
    /// </summary>
    public interface IWeb3Service
    {
        /// <summary>
        /// 取得TransactionHashEventLog
        /// </summary>
        /// <param name="transactionHash">The transaction hash.</param>
        /// <returns></returns>
        Task<List<ResponseTransactionHashEventLogModel>> GetTransactionHashEventLogAsync(string transactionHash);
    }
}
