using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ExpectedObjects;
using Microsoft.Extensions.Logging;
using model_scanner.Common;
using model_scanner.Web3;
using model_scanner.Web3.Contract;
using NUnit.Framework;
using service_scanner.Helper.Interface;
using service_scanner.Service;
using service_scanner.Service.Interface;

namespace service_scanner_test.Service
{
    public class Web3ServiceTests
    {
        private IWeb3Service _web3Service;

        private readonly IWeb3Helper _web3Helper;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<Web3Service> _logger;

        //每一次run 一個test case 跑一次
        [SetUp]
        public void Setup()
        {
            // Method intentionally left empty.
        }

        //只會建立一次
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //要使用到的
            _web3Service = new Web3Service(this._web3Helper,this._httpClientFactory,this._logger);
        }

        [Test]
        public void WhenTransactionHashIsEmpty_ReturnDefaultInstance()
        {
            var transactionHash = "";
            var expected = new ApiResponse<List<ResponseTransactionHashEventLogModel>>(new List<ResponseTransactionHashEventLogModel>()).ToExpectedObject();
            var actual = this._web3Service.GetTransactionHashEventLogAsync(transactionHash).GetAwaiter().GetResult();
            expected.ShouldEqual(actual);
        }

    }
}
