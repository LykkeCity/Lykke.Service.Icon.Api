using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Icon.Sdk;
using Lykke.Icon.Sdk.Data;
using Lykke.Quintessence.Core.Crypto;

namespace Lykke.Service.Icon.Api.Services
{
    public class KeyGenerator : IKeyGenerator
    {
        public byte[] GeneratePrivateKey()
        {
            var wallet = KeyWallet.Create();

            return wallet.GetPrivateKey().ToByteArray();
        }

        public byte[] GeneratePublicKey(byte[] privateKey)
        {
            var pkBytes = new Bytes(privateKey);
            var wallet = KeyWallet.Load(pkBytes);

            return wallet.GetPublicKey().ToByteArray();
        }
    }
}
