using System;
using System.Numerics;
using AutoMapper;
using FDex.Application.Contracts.Persistence;
using FDex.Application.DTOs.Liquidity;
using FDex.Application.DTOs.Reporter;
using FDex.Application.DTOs.Swap;
using FDex.Application.Enumerations;
using FDex.Domain.Entities;
using Microsoft.Extensions.Hosting;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Model;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace FDex.Application.Services
{
    public class EventDataSeedService : BackgroundService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Web3 _web3;

        bool isFirstParam = true;
        private static BigInteger _currentBlockNumber = 34002213;
        private static BigInteger _limitBlockNumber = 9999;
        const string RPC_URL = "https://sly-long-cherry.bsc-testnet.quiknode.pro/4ac0090884736ecd32a595fe2ec55910ca239cdb/";

        public EventDataSeedService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            ClientBase.ConnectionTimeout = TimeSpan.FromDays(1);
            _web3 = new(RPC_URL);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var latestBlockNumber = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var poolAddress = "0x006Df49bde510578dE88b75AEe4754cc86bFAFD0";
            var oracleAddress = "0x1E16D408a6ae4E2a867cd33F15cb7E17441139c1";
            var swapEventHandler = _web3.Eth.GetEvent<SwapDTO>(poolAddress);
            var addLiquidityEventHandler = _web3.Eth.GetEvent<AddLiquidityDTO>(poolAddress);
            var reporterAddedEventHandler = _web3.Eth.GetEvent<ReporterAddedDTO>(oracleAddress);
            var reporterRemovedEventHandler = _web3.Eth.GetEvent<ReporterRemovedDTO>(oracleAddress);
            var reporterPostedEventHandler = _web3.Eth.GetEvent<ReporterPostedDTO>(oracleAddress);
            while (!stoppingToken.IsCancellationRequested && _currentBlockNumber < latestBlockNumber)
            {
                BlockParameter startBlock = HandleBlockParameter();
                BlockParameter endBlock = HandleBlockParameter();

                var filterAllSwapEvents = swapEventHandler.CreateFilterInput(startBlock, endBlock);
                var filterAllAddLiquidity = addLiquidityEventHandler.CreateFilterInput(startBlock, endBlock);
                var filterAllReporterAdded = reporterAddedEventHandler.CreateFilterInput(startBlock, endBlock);
                var filterAllReporterRemoved = reporterRemovedEventHandler.CreateFilterInput(startBlock, endBlock);
                var filterAllReporterPosted = reporterPostedEventHandler.CreateFilterInput(startBlock, endBlock);

                var swapEvents = await swapEventHandler.GetAllChangesAsync(filterAllSwapEvents);
                var addLiquidityEvents = await addLiquidityEventHandler.GetAllChangesAsync(filterAllAddLiquidity);
                var reporterAddedEvents = await reporterAddedEventHandler.GetAllChangesAsync(filterAllReporterAdded);
                var reporterRemovedEvents = await reporterRemovedEventHandler.GetAllChangesAsync(filterAllReporterRemoved);
                var reporterPostedEvents = await reporterPostedEventHandler.GetAllChangesAsync(filterAllReporterPosted);


                foreach (var log in swapEvents)
                {
                    var foundUser = await _unitOfWork.UserRepository.FindAsync(log.Event.Wallet);
                    if (foundUser == null)
                    {
                        User user = new()
                        {
                            Wallet = log.Event.Wallet,
                            CreatedDate = DateTime.Now
                        };
                        await _unitOfWork.UserRepository.AddAsync(user);
                    }
                    var foundSwap = await _unitOfWork.SwapRepository.FindAsync(log.Log.TransactionHash);
                    if (foundSwap == null)
                    {
                        SwapDTOAdd rawSwap = new()
                        {
                            TxnHash = log.Log.TransactionHash,
                            Wallet = log.Event.Wallet,
                            TokenIn = log.Event.TokenIn,
                            TokenOut = log.Event.TokenOut,
                            AmountIn = log.Event.AmountIn,
                            AmountOut = log.Event.AmountOut,
                            Fee = log.Event.Fee * log.Event.MarkPrice,
                            Time = DateTime.Now
                        };
                        Swap swap = _mapper.Map<Swap>(rawSwap);
                        await _unitOfWork.SwapRepository.AddAsync(swap);
                    }
                    await _unitOfWork.SaveAsync();
                }

                foreach (var log in addLiquidityEvents)
                {
                    var foundUser = await _unitOfWork.UserRepository.FindAsync(log.Event.Wallet);
                    if (foundUser == null)
                    {
                        User user = new()
                        {
                            Wallet = log.Event.Wallet,
                            CreatedDate = DateTime.Now
                        };
                        await _unitOfWork.UserRepository.AddAsync(user);
                    }
                    var foundAddLiquidity = await _unitOfWork.AddLiquidityRepository.FindAsync(log.Log.TransactionHash);
                    if (foundAddLiquidity == null)
                    {
                        AddLiquidityDTOAdd rawAddLiquidity = new()
                        {
                            TxnHash = log.Log.TransactionHash,
                            Wallet = log.Event.Wallet,
                            Asset = log.Event.Asset,
                            Amount = log.Event.Amount,
                            Fee = log.Event.Fee * log.Event.MarkPriceIn,
                            DateAdded = DateTime.Now
                        };
                        AddLiquidity addLiquidity = _mapper.Map<AddLiquidity>(rawAddLiquidity);
                        await _unitOfWork.AddLiquidityRepository.AddAsync(addLiquidity);
                    }
                    await _unitOfWork.SaveAsync();
                }

                foreach (var log in reporterAddedEvents)
                {
                    await HandleReporterEvent(log.Event.Wallet, ReporterEventType.Added);
                }
                foreach (var log in reporterRemovedEvents)
                {
                    await HandleReporterEvent(log.Event.Wallet, ReporterEventType.Removed);
                }
                foreach (var log in reporterPostedEvents)
                {
                    await HandleReporterEvent(log.Event.Wallet, ReporterEventType.Posted);
                }
            }
        }

        private BlockParameter HandleBlockParameter()
        {
            if (isFirstParam)
            {
                _currentBlockNumber += 1;
                isFirstParam = !isFirstParam;
                return new BlockParameter(new HexBigInteger(_currentBlockNumber));
            }
            _currentBlockNumber += _limitBlockNumber;
            return new BlockParameter(new HexBigInteger(_currentBlockNumber));
        }

        private async Task HandleReporterEvent(string wallet, ReporterEventType reporterEvent)
        {
            switch (reporterEvent)
            {
                case ReporterEventType.Added:
                    var foundReporterAdded = await _unitOfWork.ReporterRepository.FindAsync(wallet);
                    if (foundReporterAdded == null)
                    {
                        await _unitOfWork.ReporterRepository.AddAsync(new Reporter { Wallet = wallet, ReportCount = 0 });
                    }
                    break;
                case ReporterEventType.Removed:
                    var foundReporterRemoved = await _unitOfWork.ReporterRepository.FindAsync(wallet);
                    if (foundReporterRemoved != null)
                    {
                        _unitOfWork.ReporterRepository.Remove(foundReporterRemoved);
                    }
                    break;
                case ReporterEventType.Posted:
                    var foundReporterPosted = await _unitOfWork.ReporterRepository.FindAsync(wallet);
                    if (foundReporterPosted != null)
                    {
                        Reporter postingReporter = await _unitOfWork.ReporterRepository.FindAsync(wallet);
                        postingReporter.ReportCount += 1;
                        postingReporter.LastReportedDate = DateTime.Now;
                        _unitOfWork.ReporterRepository.Update(postingReporter);
                    }
                    break;
            }
            await _unitOfWork.SaveAsync();
        }
    }
}

