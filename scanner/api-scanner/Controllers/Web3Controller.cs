using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using model_scanner.Common;
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
        public ApiResponse<bool> GetEventLogByTransactionHash(string transactionHash)
        {
            return new ApiResponse<bool>(true);
        }
    }
}