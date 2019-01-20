using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Quintessence.Domain.Services;

namespace Lykke.Service.Icon.Api.Services
{
    public class IconAssetService : DefaultAssetServiceBase
    {
        public IconAssetService() : base(18, "", "ICX", "ICX")
        {

        }
    }
}
