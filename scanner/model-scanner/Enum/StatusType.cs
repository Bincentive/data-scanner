using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace model_scanner.Enum
{
    /// <summary>
    /// StatusType
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("Success.")]
        Success = 0,
        /// <summary>
        /// 輸入資料異常
        /// </summary>
        [Description("Inport Data Exception.")]
        InportDataException = 1000,
        /// <summary>
        /// 遠端API發生異常
        /// </summary>
        [Description("Remote Api Exception.")]
        RemoteApiException = 9998,
    }
}
