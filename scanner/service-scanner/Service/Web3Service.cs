using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using model_scanner.Common;
using model_scanner.Enum;
using model_scanner.Etherscan;
using model_scanner.Web3;
using model_scanner.Web3.Contract;
using Nethereum.JsonRpc.Client;
using service_scanner.Helper.Interface;
using service_scanner.Service.Interface;

namespace service_scanner.Service
{
    /// <summary>
    /// Web3Service
    /// </summary>
    public class Web3Service:IWeb3Service
    {
        /// <summary>
        /// IWeb3Helper
        /// </summary>
        private readonly IWeb3Helper _web3Helper;
        /// <summary>
        /// IHttpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;
        /// <summary>
        /// ILogger
        /// </summary>
        private readonly ILogger<Web3Service> _logger;
        /// <summary>
        /// Web3Service
        /// </summary>
        /// <param name="web3Helper">IWeb3Helper</param>
        /// <param name="httpClientFactory">IHttpClientFactory</param>
        public Web3Service(IWeb3Helper web3Helper,IHttpClientFactory httpClientFactory,ILogger<Web3Service> logger)
        {
            this._web3Helper = web3Helper;
            this._httpClientFactory = httpClientFactory;
            this._logger = logger;
        }

        /// <summary>
        /// 取得TransactionHashEventLog
        /// </summary>
        /// <param name="transactionHash">The transaction hash.</param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ResponseTransactionHashEventLogModel>>> GetTransactionHashEventLogAsync(string transactionHash)
        {
            var result = new List<ResponseTransactionHashEventLogModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(transactionHash))
                {
                    return new ApiResponse<List<ResponseTransactionHashEventLogModel>>(result);
                }
                var eventLogs =
                    await this._web3Helper.GetTransactionReceiptForEventLogsAsync<EventEventDTO>(transactionHash);
                foreach (var item in eventLogs)
                {
                    try
                    {
                        var eventLog =
                            System.Text.Json.JsonSerializer.Deserialize<ResponseTransactionHashEventLogModel>(item.Event
                                .tradingTx);
                        result.Add(eventLog);
                    }
                    catch (Exception e)
                    {
                        this._logger.LogError(item.Event.tradingTx+":"+e.InnerException?.ToString());
                        continue;
                    }
                }

                return new ApiResponse<List<ResponseTransactionHashEventLogModel>>(result);
            }
            catch (Nethereum.JsonRpc.Client.RpcResponseException e)
            {
                this._logger.LogError(e.InnerException?.ToString());
                return new ApiResponse<List<ResponseTransactionHashEventLogModel>>(StatusType.RemoteApiException,
                    result, "TransactionHash Error.");
            }
            catch (Exception)
            {
                this._logger.LogError("TransactionHash: " + transactionHash + ",Format Json Exception.");
                return new ApiResponse<List<ResponseTransactionHashEventLogModel>>(result);
            }
        }

        /// <summary>
        /// 取得 Etherscan Transaction Log
        /// </summary>
        /// <param name="contractAddress">Contract address</param>
        /// <param name="fromBlock">block</param>
        /// <param name="topic0">topic0</param>
        /// <param name="apiKey">Etherscan apikey</param>
        /// <returns></returns>
        public async Task<ApiResponse<List<ResponseContractLogResultModel>>> GetEtherscanTransactionByContractAddress(
            string contractAddress, long? fromBlock,string topic0,string apiKey)
        {
            var result = new List<ResponseContractLogResultModel>();
            
            if (string.IsNullOrWhiteSpace(contractAddress))
            {
                return new ApiResponse<List<ResponseContractLogResultModel>>(StatusType.InportDataException, result,
                    "Contract Address is Null.");
            }
            var queryString = new StringBuilder();
            queryString.Append("&address=" + contractAddress);

            if (fromBlock.HasValue)
            {
                if (fromBlock != 0)
                {
                    if (fromBlock >= long.MaxValue)
                    {
                        queryString.Append("&fromBlock=" + long.MaxValue);
                    }
                    else
                    {
                        queryString.Append("&fromBlock=" + fromBlock);
                    }
                }
            }
            else
            {
                queryString.Append("&fromBlock=0");
            }

            if (string.IsNullOrWhiteSpace(apiKey) == false)
            {
                queryString.Append("&apikey="+apiKey);
            }

            if (string.IsNullOrWhiteSpace(topic0) == false)
            {
                queryString.Append("&topic0=" + topic0);
            }

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using (var client = _httpClientFactory.CreateClient())
            {
                client.BaseAddress = new Uri("https://api.etherscan.io");
                var apiResult = await client
                    .SendAsync(
                        new HttpRequestMessage(HttpMethod.Get,
                            "/api?module=logs&action=getLogs&toBlock=latest" + queryString),
                        HttpCompletionOption.ResponseHeadersRead, cts.Token).ConfigureAwait(false);
                using (var contentStream = await apiResult.Content.ReadAsStreamAsync())
                {
                    var apiResultModel = await JsonSerializer.DeserializeAsync<ResponseContractLogModel>(contentStream, cancellationToken: cts.Token);
                    if (apiResultModel.Status == "1")
                    {
                        return new ApiResponse<List<ResponseContractLogResultModel>>(apiResultModel.Result);
                    }
                    return new ApiResponse<List<ResponseContractLogResultModel>>(StatusType.RemoteApiException, result, apiResultModel.Message);
                }
                
               
            }
                
        }
    }
}
