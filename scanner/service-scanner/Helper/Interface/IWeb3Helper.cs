using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Contracts;

namespace service_scanner.Helper.Interface
{
    public interface IWeb3Helper
    {
        /// <summary>
        /// 取得TransactionReceiptLogs
        /// </summary>
        /// <typeparam name="TEventLogDto">The type of the event log dto.</typeparam>
        /// <param name="transactionHash">The transaction hash.</param>
        /// <returns></returns>
        Task<List<EventLog<TEventLogDto>>> GetTransactionReceiptForEventLogsAsync<TEventLogDto>(string transactionHash) where TEventLogDto : new();
    }
}
