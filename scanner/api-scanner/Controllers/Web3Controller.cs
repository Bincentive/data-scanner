using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using model_scanner.Common;
using model_scanner.Etherscan;
using model_scanner.Web3;
using service_scanner.Service.Interface;

namespace api_scanner.Controllers
{
    /// <summary>
    /// Web3Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Web3Controller : ControllerBase
    {
        /// <summary>
        /// IWeb3Service
        /// </summary>
        private readonly IWeb3Service _web3Service;

        /// <summary>
        /// Web3Controller
        /// </summary>
        public Web3Controller(IWeb3Service web3Service)
        {
            this._web3Service = web3Service;
        }

        /// <summary>
        /// Get TransactionHash EventLog
        /// </summary>
        /// <returns></returns>
        [HttpGet("eventLog/transactionHash/{transactionHash}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<ResponseTransactionHashEventLogModel>>))]
        public async Task<ApiResponse<List<ResponseTransactionHashEventLogModel>>> GetEventLogByTransactionHashAsync(string transactionHash)
        {
            var result = await this._web3Service.GetTransactionHashEventLogAsync(transactionHash);
            return result;
        }

        /// <summary>
        /// Get Contract Events TxnHash Async
        /// </summary>
        /// <param name="contractAddress">合約地址</param>
        /// <param name="fromBlock">從哪一個Block數</param>
        /// <param name="topic0">topic0</param>
        /// <param name="apiKey">Etherscan申請的ApiKey</param>
        /// <returns></returns>
        /// <remarks>
        /// ApiKey:None(rate limit of 5 requests/sec)
        /// </remarks>
        [HttpGet("contract/{contractAddress}/transactionHash")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<ResponseContractLogResultModel>>))]
        public async Task<ApiResponse<List<ResponseContractLogResultModel>>> GetContractEventsTxnHashAsync(
            string contractAddress, long? fromBlock, string topic0, string apiKey)
        {

            var result =
                await this._web3Service.GetEtherscanTransactionByContractAddress(contractAddress, fromBlock, topic0,
                    apiKey);
            return result;
        }
    }
}