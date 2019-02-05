using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Icon.Sdk.Crypto;
using Lykke.Quintessence.Domain;
using Lykke.Quintessence.Domain.Repositories;
using Lykke.Quintessence.Domain.Services;
using Lykke.Quintessence.Domain.Services.Strategies;

namespace Lykke.Service.Icon.Api.Services
{
    public class IconAddressService : IAddressService
    {
        private readonly IBlockchainService _blockchainService;
        private readonly IWhitelistedAddressRepository _whitelistedAddressRepository;
        private readonly IBlacklistedAddressRepository _blacklistedAddressRepository;
        private readonly ILog _log;

        public IconAddressService(
            IBlacklistedAddressRepository blacklistedAddressRepository,
            IBlockchainService blockchainService,
            ILogFactory logFactory,
            IWhitelistedAddressRepository whitelistedAddressRepository)
        {
            _blockchainService = blockchainService;
            _whitelistedAddressRepository = whitelistedAddressRepository;
            _blacklistedAddressRepository = blacklistedAddressRepository;
            _log = logFactory.CreateLog(this);
        }

        public async Task<AddAddressResult> AddAddressToBlacklistAsync(
            string address,
            string reason)
        {
            var addressHasBeenAdded = await _blacklistedAddressRepository.AddIfNotExistsAsync
            (
                address: address,
                reason: reason
            );

            if (addressHasBeenAdded)
            {
                _log.Info
                (
                    $"Address [{address}] has been blacklisted.",
                    new { address }
                );

                return AddAddressResult.Success();
            }
            else
            {
                return AddAddressResult.HasAlreadyBeenAdded();
            }
        }

        public async Task<AddAddressResult> AddAddressToWhitelistAsync(
            string address,
            BigInteger maxGasAmount)
        {
            var addressHasBeenAdded = await _whitelistedAddressRepository.AddIfNotExistsAsync
            (
                address,
                maxGasAmount
            );

            if (addressHasBeenAdded)
            {
                _log.Info
                (
                    $"Address [{address}] has been whitelisted.",
                    new { address }
                );

                return AddAddressResult.Success();
            }
            else
            {
                return AddAddressResult.HasAlreadyBeenAdded();
            }
        }

        public Task<(IReadOnlyCollection<BlacklistedAddress> BlacklistedAddresses, string ContinuationToken)> GetBlacklistedAddressesAsync(int take, string continuationToken)
        {
            return _blacklistedAddressRepository.GetAllAsync(take, continuationToken);
        }

        public Task<(IReadOnlyCollection<WhitelistedAddress> WhitelistedAddresses, string ContinuationToken)> GetWhitelistedAddressesAsync(int take, string continuationToken)
        {
            return _whitelistedAddressRepository.GetAllAsync(take, continuationToken);
        }

        public async Task<RemoveAddressResult> RemoveAddressFromBlacklistAsync(
            string address)
        {
            var addressHasBeenRemoved = await _blacklistedAddressRepository.RemoveIfExistsAsync(address);

            if (addressHasBeenRemoved)
            {
                _log.Info
                (
                    $"Address [{address}] has been removed from blacklist.",
                    new { address }
                );

                return RemoveAddressResult.Success();
            }
            else
            {
                return RemoveAddressResult.NotFound();
            }
        }

        public async Task<RemoveAddressResult> RemoveAddressFromWhitelistAsync(
            string address)
        {
            var addressHasBeenRemoved = await _whitelistedAddressRepository.RemoveIfExistsAsync(address);

            if (addressHasBeenRemoved)
            {
                _log.Info
                (
                    $"Address [{address}] has been removed from whitelist.",
                    new { address }
                );

                return RemoveAddressResult.Success();
            }
            else
            {
                return RemoveAddressResult.NotFound();
            }
        }

        public async Task<string> TryGetBlacklistingReason(
            string address)
        {
            return (await _blacklistedAddressRepository.TryGetAsync(address))?
                .BlacklistingReason;
        }

        public async Task<BigInteger?> TryGetCustomMaxGasAmountAsync(
            string address)
        {
            return (await _whitelistedAddressRepository.TryGetAsync(address))?
                .MaxGasAmount;
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
