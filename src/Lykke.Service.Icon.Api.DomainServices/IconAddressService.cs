using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Lykke.Common.Log;
using Lykke.Icon.Sdk.Crypto;
using Lykke.Quintessence.Domain;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Service.Icon.Api.Services
{
    public class IconAddressService : DefaultAddressService
    {
        private readonly IBlockchainService _blockchainService;
        private readonly IWhitelistedAddressRepository _whitelistedAddressRepository;
        private readonly IBlacklistedAddressRepository _blacklistedAddressRepository;

        public IconAddressService(
            IAddChecksumStrategy addChecksumStrategy, 
            IBlacklistedAddressRepository blacklistedAddressRepository, 
            IBlockchainService blockchainService, 
            ILogFactory logFactory, 
            IValidateChecksumStrategy validateChecksumStrategy, 
            IWhitelistedAddressRepository whitelistedAddressRepository) : 
            base(
                addChecksumStrategy, 
                blacklistedAddressRepository,
                blockchainService,
                logFactory,
                validateChecksumStrategy,
                whitelistedAddressRepository)
        {
            _blockchainService = blockchainService;
            _whitelistedAddressRepository = whitelistedAddressRepository;
            _blacklistedAddressRepository = blacklistedAddressRepository;
        }

        public new Task<string> AddChecksumAsync(string address)
        {
            return Task.FromResult(address);
        }

        public new async Task<AddressValidationResult> ValidateAsync(
            string address,
            bool skipChecksumValidation,
            bool skipWhiteAndBlacklistCheck)
        {
            if (!IconKeys.IsValidAddress(address))
            {
                return AddressValidationResult.AddressIsInvalid(AddressValidationError.FormatIsInvalid);
            }

            if (skipWhiteAndBlacklistCheck)
            {
                address = address.ToLowerInvariant();

                if (await _blockchainService.IsContractAsync(address) &&
                    !await _whitelistedAddressRepository.ContainsAsync(address) &&
                    await _blacklistedAddressRepository.ContainsAsync(address))
                {
                    return AddressValidationResult.AddressIsInvalid(AddressValidationError.AddressIsBlacklisted);
                }
            }

            return AddressValidationResult.AddressIsValid();
        }
    }
}
