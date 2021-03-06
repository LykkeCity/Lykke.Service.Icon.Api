﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Icon.Sdk;
using Lykke.Quintessence.Core.Blockchain;

namespace Lykke.Service.Icon.Api.Services
{
    public class WalletGenerator : IWalletGenerator
    {
        private const string nullAddresContext = null;

        public Task<(string Address, string AddressContext, string PrivateKey)> GenerateWalletAsync()
        {
            var wallet = KeyWallet.Create();
            var address = wallet.GetAddress().ToString();
            var privateKey = wallet.GetPrivateKey().ToHexString(false);

            return Task.FromResult((address, nullAddresContext, privateKey));
        }
    }
}
