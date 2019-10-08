using System;
using System.Collections.Generic;
using System.Text;
using service_scanner.Helper.Interface;
using service_scanner.Service.Interface;

namespace service_scanner.Service
{
    public class Web3Service:IWeb3Service
    {
        private readonly IWeb3Helper _web3Helper;
        public Web3Service(IWeb3Helper web3Helper)
        {
            this._web3Helper = web3Helper;
        }
    }
}
