using System;
using System.Collections.Generic;
using System.Text;
using model_scanner.Enum;

namespace model_scanner.Common
{
    /// <summary>
    /// ApiResponse
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class ApiResponse<TData>
    {
        /// <summary>
        /// 狀態
        /// </summary>
        public StatusType Status { get; set; }
        /// <summary>
        /// 回傳資料
        /// </summary>
        public TData Data { get; set; }
        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{TData}"/> class.
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="data">data</param>
        /// <param name="message">message</param>
        public ApiResponse(StatusType status, TData data,string message)
        {
            this.Status = status;
            this.Data = data;
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{TData}"/> class.
        /// </summary>
        /// <param name="data">data</param>
        public ApiResponse(TData data)
        {
            this.Status = StatusType.Success;
            this.Data = data;
            this.Message = "";
        }
    }
}
