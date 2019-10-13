using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using model_scanner.Common;
using model_scanner.Etherscan;
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
        Task<ApiResponse<List<ResponseTransactionHashEventLogModel>>> GetTransactionHashEventLogAsync(string transactionHash);

        /// <summary>
        /// 取得 Etherscan Transaction Log
        /// </summary>
        /// <param name="contractAddress">Contract address</param>
        /// <param name="fromBlock">block</param>
        /// <param name="topic0">topic0</param>
        /// <param name="apiKey">Etherscan apikey</param>
        /// <returns></returns>
        Task<ApiResponse<List<ResponseContractLogResultModel>>> GetEtherscanTransactionByContractAddress(
            string contractAddress, long? fromBlock, string topic0, string apiKey);
    }
}
