﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Service.Icon.Api.Core.Domain;
using Lykke.Service.Icon.Api.Core.Services;
using System.Numerics;
using Lykke.Service.Icon.Api.Core.Repositories;

namespace Lykke.Service.Icon.Api.Services
{
    [UsedImplicitly]
    public class TransactionService : ITransactionService
    {
        private readonly IAddressValidationService _addressService;
        private readonly IBlockchainService _blockchainService;
        private readonly IChaosKitty _chaosKitty;
        private readonly ILog _log;
        private readonly ITransactionMonitoringTaskRepository _transactionMonitoringTaskRepository;
        private readonly ITransactionRepository _transactionRepository;

        private readonly IBuildTransactionStrategy _buildTransactionStrategy;
        private readonly ICalculateGasAmountStrategy _calculateGasAmountStrategy;
        private readonly ICalculateTransactionAmountStrategy _calculateTransactionAmountStrategy;


        public TransactionService(
            IAddressValidationService addressService,
            IBlockchainService blockchainService,
            IChaosKitty chaosKitty,
            ILogFactory logFactory,
            ITransactionMonitoringTaskRepository transactionMonitoringTaskRepository,
            ITransactionRepository transactionRepository,
            IBuildTransactionStrategy buildTransactionStrategy,
            ICalculateGasAmountStrategy calculateGasAmountStrategy,
            ICalculateTransactionAmountStrategy calculateTransactionAmountStrategy)
        {
            _addressService = addressService;
            _blockchainService = blockchainService;
            _chaosKitty = chaosKitty;
            _log = logFactory.CreateLog(this);
            _transactionMonitoringTaskRepository = transactionMonitoringTaskRepository;
            _transactionRepository = transactionRepository;
            _buildTransactionStrategy = buildTransactionStrategy;
            _calculateGasAmountStrategy = calculateGasAmountStrategy;
            _calculateTransactionAmountStrategy = calculateTransactionAmountStrategy;
        }


        public async Task<BuildTransactionResult> BuildTransactionAsync(
            Guid transactionId,
            string from,
            string to,
            BigInteger transferAmount,
            bool includeFee)
        {
            var transaction = await _transactionRepository.TryGetAsync(transactionId);

            if (transaction == null)
            {
                // Check address validity

                var addressValidationResult = await _addressService.ValidateAsync(to, false, false);

                if (addressValidationResult is AddressValidationResult.Error error)
                {
                    _log.Info
                    (
                        $"Failed to build transaction [{transactionId}]. Target address [{to}] is invalid. {error.Type.ToReason()}",
                        new { transactionId, from, to }
                    );

                    return BuildTransactionResult.TargetAddressIsInvalid();
                }

                // Calculate and validate required gas amount

                var gasAmountCalculationResult = await _calculateGasAmountStrategy.ExecuteAsync
                (
                    addressService: _addressService,
                    blockchainService: _blockchainService,
                    from: from,
                    to: to,
                    transferAmount: transferAmount
                );

                if (gasAmountCalculationResult is GasAmountCalculationResult.ErrorResult)
                {
                    _log.Info
                    (
                        $"Failed to build transaction [{transactionId}]. Gas amount is invalid. {gasAmountCalculationResult}",
                        new { transactionId, from, to }
                    );

                    return BuildTransactionResult.GasAmountIsInvalid();
                }

                var gasAmount = (BigInteger)gasAmountCalculationResult;

                // Calculate and validate transaction amount

                var transactionAmountCalculationResult = await _calculateTransactionAmountStrategy.ExecuteAsync
                (
                    blockchainService: _blockchainService,
                    from: from,
                    transferAmount: transferAmount,
                    gasAmount: gasAmount,
                    includeFee: includeFee
                );

                switch (transactionAmountCalculationResult)
                {
                    case TransactionAmountCalculationResult.TransactionAmountIsTooSmallResult _:
                        _log.Info
                        (
                            $"Failed to build transaction [{transactionId}]: amount is too small.",
                            new { transactionId, from, to }
                        );

                        return BuildTransactionResult.AmountIsTooSmall();

                    case TransactionAmountCalculationResult.BalanceIsNotEnoughResult _:
                        _log.Info
                        (
                            $"Failed to build transaction [{transactionId}]: balance is not enough.",
                            new { transactionId, from, to }
                        );

                        return BuildTransactionResult.BalanceIsNotEnough();
                }

                var transactionAmount = (BigInteger)transactionAmountCalculationResult;

                // Build transaction

                var transactionData = await _buildTransactionStrategy.ExecuteAsync
                (
                    transactionRepository: _transactionRepository,
                    blockchainService: _blockchainService,
                    transactionId: transactionId,
                    from: from,
                    to: to,
                    transactionAmount: transactionAmount,
                    gasAmount: gasAmount,
                    includeFee: includeFee
                );

                _log.Info
                (
                    $"Transaction [{transactionId}] has been built.",
                    new { transactionId, from, to }
                );

                return BuildTransactionResult.Success(transactionData);
            }
            else
            {
                switch (transaction.State)
                {
                    case TransactionState.Built:
                        return BuildTransactionResult.Success(transaction.Data);

                    case TransactionState.InProgress:
                    case TransactionState.Completed:
                    case TransactionState.Failed:
                        return BuildTransactionResult.TransactionHasBeenBroadcasted();

                    case TransactionState.Deleted:
                        return BuildTransactionResult.TransactionHasBeenDeleted();

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public async Task<BroadcastTransactionResult> BroadcastTransactionAsync(
            Guid transactionId,
            string signedTxData)
        {
            var transaction = await _transactionRepository.TryGetAsync(transactionId);

            if (transaction != null)
            {
                switch (transaction.State)
                {
                    case TransactionState.Built:

                        var txHash = await _blockchainService.BroadcastTransactionAsync(signedTxData);

                        transaction.OnBroadcasted
                        (
                            hash: txHash,
                            signedData: signedTxData
                        );

                        await _transactionMonitoringTaskRepository.EnqueueAsync
                        (
                            new TransactionMonitoringTask(transactionId),
                            TimeSpan.FromMinutes(1)
                        );

                        _chaosKitty.Meow(transactionId);

                        await _transactionRepository.UpdateAsync(transaction);

                        _chaosKitty.Meow(transactionId);

                        return BroadcastTransactionResult.Success(txHash);

                    case TransactionState.InProgress:
                    case TransactionState.Completed:
                    case TransactionState.Failed:
                        return BroadcastTransactionResult.TransactionHasBeenBroadcasted();

                    case TransactionState.Deleted:
                        return BroadcastTransactionResult.TransactionHasBeenDeleted();

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                return BroadcastTransactionResult.TransactionHasNotBeenFound();
            }
        }

        public async Task<bool> DeleteTransactionIfExistsAsync(
            Guid transactionId)
        {
            var transaction = await _transactionRepository.TryGetAsync(transactionId);

            if (transaction != null)
            {
                transaction.OnDeleted();

                await _transactionRepository.UpdateAsync(transaction);

                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<Transaction> TryGetTransactionAsync(
            Guid transactionId)
        {
            return _transactionRepository.TryGetAsync(transactionId);
        }
    }
}
