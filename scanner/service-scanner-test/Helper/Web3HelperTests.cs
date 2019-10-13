using System;
using System.Collections.Generic;
using System.Text;
using model_scanner.Web3.Contract;
using Nethereum.Contracts;
using NUnit.Framework;
using service_scanner.Helper;
using service_scanner.Helper.Interface;
using service_scanner_test.SourceData.Helper;
using ExpectedObjects;

namespace service_scanner_test.Helper
{
    public class Web3HelperTests
    {
        private IWeb3Helper _web3Helper;

        //每一次run 一個test case 跑一次
        [SetUp]
        public void Setup()
        {
        }

        //只會建立一次
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //要使用到的
            this._web3Helper = new Web3Helper("");
        }

        [Test]
        public void WhenTransactionHashIsEmpty_ReturnEmptyInstance()
        {
            var transactionHash = "";
            
            var expected = new List<EventLog<EventEventDTO>>().ToExpectedObject();
            var actual = this._web3Helper.GetTransactionReceiptForEventLogsAsync<EventEventDTO>(transactionHash).Result;
            expected.ShouldEqual(actual);
        }
    }
}
