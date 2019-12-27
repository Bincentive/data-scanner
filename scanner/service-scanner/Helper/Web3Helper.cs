using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Nethereum.Contracts;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using service_scanner.Helper.Interface;

namespace service_scanner.Helper
{
    /// <summary>
    /// Web3
    /// </summary>
    public class Web3Helper : IWeb3Helper, IDisposable
    {
        /// <summary>
        /// The web3
        /// </summary>
        public Web3 _Web3;

        public Web3Helper(string rpcServer)
        {
            this._Web3 = rpcServer == "" ? new Web3() : new Web3(rpcServer);
        }

        /// <summary>
        /// 取得TransactionReceiptLogs
        /// </summary>
        /// <typeparam name="TEventLogDto">The type of the event log dto.</typeparam>
        /// <param name="transactionHash">The transaction hash.</param>
        /// <returns></returns>
        public async Task<List<EventLog<TEventLogDto>>> GetTransactionReceiptForEventLogsAsync<TEventLogDto>(string transactionHash) where TEventLogDto : new()
        {
            if (string.IsNullOrWhiteSpace(transactionHash))
            {
                return new List<EventLog<TEventLogDto>>();
            }
            var receipt = await _Web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            if (receipt == null)
            {
                return new List<EventLog<TEventLogDto>>();
            }

            var eventLogs = receipt.Logs.DecodeAllEvents<TEventLogDto>();
            return eventLogs;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 偵測多餘的呼叫

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置 Managed 狀態 (Managed 物件)。
                    this._Web3 = null;
                }

                // TODO: 釋放 Unmanaged 資源 (Unmanaged 物件) 並覆寫下方的完成項。
                // TODO: 將大型欄位設為 null。

                disposedValue = true;
            }
        }

        // TODO: 僅當上方的 Dispose(bool disposing) 具有會釋放 Unmanaged 資源的程式碼時，才覆寫完成項。
        // ~Web3Helper()
        // {
        //   // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 加入這個程式碼的目的在正確實作可處置的模式。
        public void Dispose()
        {
            // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果上方的完成項已被覆寫，即取消下行的註解狀態。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
