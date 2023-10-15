using System;
using AutoMapper;
using FDex.Application.Contracts.Persistence;
using FDex.Application.DTOs.Liquidity;
using FDex.Application.DTOs.Reporter;
using FDex.Application.DTOs.Swap;
using FDex.Application.DTOs.TradingPosition;
using FDex.Application.Enumerations;
using FDex.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nethereum.ABI.Model;
using Nethereum.Contracts;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Reactive.Eth.Subscriptions;

namespace FDex.Application.Services
{
    public class EventDispatcherService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly List<string> _wss;
        private int currentWssIndex;

        public EventDispatcherService(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _wss = new()
            {
                "wss://bsc-testnet.publicnode.com",
                "wss://bsc.getblock.io/c9c2311d-f632-47b1-ae8f-7cde9cd02fba/testnet/",
                "wss://bsc.getblock.io/034447a0-2d47-427d-a049-27bca272aeb0/testnet/",
                "wss://bsc-testnet.public.blastapi.io",
                "wss://sly-long-cherry.bsc-testnet.quiknode.pro/4ac0090884736ecd32a595fe2ec55910ca239cdb/"

            };
            currentWssIndex = 0;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                try
                {
                    StreamingWebSocketClient client = new(HandleWebsocketString());

                    NewFilterInput swapFilterInput = Event<SwapDTO>.GetEventABI().CreateFilterInput();
                    NewFilterInput addLiquidityFilterInput = Event<AddLiquidityDTO>.GetEventABI().CreateFilterInput();
                    NewFilterInput reporterAddedFilterInput = Event<ReporterAddedDTO>.GetEventABI().CreateFilterInput();
                    NewFilterInput reporterRemovedFilterInput = Event<ReporterRemovedDTO>.GetEventABI().CreateFilterInput();
                    NewFilterInput reporterPostedFilterInput = Event<ReporterPostedDTO>.GetEventABI().CreateFilterInput();
                    NewFilterInput increasePositionFilterInput = Event<IncreasePositionDTO>.GetEventABI().CreateFilterInput();
                    NewFilterInput decreasePositionFilterInput = Event<DecreasePositionDTO>.GetEventABI().CreateFilterInput();
                    NewFilterInput updatePositionFilterInput = Event<UpdatePositionDTO>.GetEventABI().CreateFilterInput();
                    NewFilterInput closePositionFilterInput = Event<ClosePositionDTO>.GetEventABI().CreateFilterInput();
                    NewFilterInput liquidatePositionFilterInput = Event<LiquidatePositionDTO>.GetEventABI().CreateFilterInput();

                    EthLogsObservableSubscription swapSubscription = new(client);
                    EthLogsObservableSubscription addLiquiditySubscription = new(client);
                    EthLogsObservableSubscription reporterAddedSubscription = new(client);
                    EthLogsObservableSubscription reporterRemovedSubscription = new(client);
                    EthLogsObservableSubscription reporterPostedSubscription = new(client);
                    EthLogsObservableSubscription increasePositionSubscription = new(client);
                    EthLogsObservableSubscription decreasePositionSubscription = new(client);
                    EthLogsObservableSubscription updatePositionSubscription = new(client);
                    EthLogsObservableSubscription closePositionSubscription = new(client);
                    EthLogsObservableSubscription liquidatePositionSubscription = new(client);
                    swapSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        var decodedSwap = Event<SwapDTO>.DecodeEvent(log);
                        if (decodedSwap != null)
                        {
                            Console.WriteLine("[DEV-INF] Decoding a swap event ...");
                            SwapDTOAdd swapDTOAdd = new()
                            {
                                TxnHash = decodedSwap.Log.TransactionHash,
                                Wallet = decodedSwap.Event.Wallet,
                                TokenIn = decodedSwap.Event.TokenIn,
                                TokenOut = decodedSwap.Event.TokenOut,
                                AmountIn = decodedSwap.Event.AmountIn,
                                AmountOut = decodedSwap.Event.AmountOut,
                                Fee = decodedSwap.Event.Fee * decodedSwap.Event.MarkPrice,
                                Time = DateTime.Now
                            };
                            Swap swap = _mapper.Map<Swap>(swapDTOAdd);
                            await _unitOfWork.SwapRepository.AddAsync(swap);
                            var foundUser = await _unitOfWork.UserRepository.FindAsync(decodedSwap.Event.Wallet);
                            if (foundUser == null)
                            {
                                User user = new()
                                {
                                    Wallet = decodedSwap.Event.Wallet,
                                    CreatedDate = DateTime.Now
                                };
                                await _unitOfWork.UserRepository.AddAsync(user);
                            }
                            await _unitOfWork.SaveAsync();
                        }
                        else
                        {
                            Console.WriteLine("[DEV-INF] Found not standard event log");
                        }
                    });

                    reporterAddedSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        Console.WriteLine("[DEV-INF] Decoding a reporter added event ...");
                        var decodedReporterAdded = Event<ReporterAddedDTO>.DecodeEvent(log);
                        var foundReporterAdded = await _unitOfWork.ReporterRepository.FindAsync(decodedReporterAdded.Event.Wallet);
                        if (foundReporterAdded == null)
                        {
                            await _unitOfWork.ReporterRepository.AddAsync(new Reporter { Wallet = decodedReporterAdded.Event.Wallet, ReportCount = 0 });
                        }
                        await _unitOfWork.SaveAsync();
                    });

                    reporterRemovedSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        Console.WriteLine("[DEV-INF] Decoding a reporter removed event ...");
                        var decodedReporterRemoved = Event<ReporterRemovedDTO>.DecodeEvent(log);
                        var foundReporterRemoved = await _unitOfWork.ReporterRepository.FindAsync(decodedReporterRemoved.Event.Wallet);
                        if (foundReporterRemoved != null)
                        {
                            _unitOfWork.ReporterRepository.Remove(foundReporterRemoved);
                        }
                        await _unitOfWork.SaveAsync();
                    });

                    reporterPostedSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        Console.WriteLine("[DEV-INF] Decoding a reporter posted event ...");
                        var decodedReporterPosted = Event<ReporterPostedDTO>.DecodeEvent(log);
                        var foundReporterPosted = await _unitOfWork.ReporterRepository.FindAsync(decodedReporterPosted.Event.Wallet);
                        if (foundReporterPosted != null)
                        {
                            Reporter postingReporter = await _unitOfWork.ReporterRepository.FindAsync(decodedReporterPosted.Event.Wallet);
                            postingReporter.ReportCount += 1;
                            postingReporter.LastReportedDate = DateTime.Now;
                            _unitOfWork.ReporterRepository.Update(postingReporter);
                        }
                        await _unitOfWork.SaveAsync();
                    });

                    addLiquiditySubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        var decodedAddLiquidity = Event<AddLiquidityDTO>.DecodeEvent(log);
                        if (decodedAddLiquidity != null)
                        {
                            Console.WriteLine("[DEV-INF] Decoding an add liquidity event ...");
                            var foundUser = await _unitOfWork.UserRepository.FindAsync(decodedAddLiquidity.Event.Wallet);
                            if (foundUser == null)
                            {
                                User user = new()
                                {
                                    Wallet = decodedAddLiquidity.Event.Wallet,
                                    CreatedDate = DateTime.Now
                                };
                                await _unitOfWork.UserRepository.AddAsync(user);
                                await _unitOfWork.SaveAsync();
                            }
                            AddLiquidityDTOAdd addLiquidityDTOAdd = new()
                            {
                                TxnHash = decodedAddLiquidity.Log.TransactionHash,
                                Wallet = decodedAddLiquidity.Event.Wallet,
                                Asset = decodedAddLiquidity.Event.Asset,
                                Amount = decodedAddLiquidity.Event.Amount,
                                Fee = decodedAddLiquidity.Event.Fee * decodedAddLiquidity.Event.MarkPriceIn,
                                DateAdded = DateTime.Now
                            };
                            AddLiquidity addLiquidity = _mapper.Map<AddLiquidity>(addLiquidityDTOAdd);
                            await _unitOfWork.AddLiquidityRepository.AddAsync(addLiquidity);
                            await _unitOfWork.SaveAsync();
                        }
                        else
                        {
                            Console.WriteLine("[DEV-INF] Found not standard event log");
                        }
                    });

                    increasePositionSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        var decodedIncreasePosition = Event<IncreasePositionDTO>.DecodeEvent(log);
                        if (decodedIncreasePosition != null)
                        {
                            Console.WriteLine("[DEV-INF] Decoding an increase position event ...");
                        }
                        else
                        {
                            Console.WriteLine("[DEV-INF] Found not standard event log");
                        }
                    });

                    decreasePositionSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        var decodedDecreasePosition = Event<DecreasePositionDTO>.DecodeEvent(log);
                        if (decodedDecreasePosition != null)
                        {
                            Console.WriteLine("[DEV-INF] Decoding a decrease position event ...");
                        }
                        else
                        {
                            Console.WriteLine("[DEV-INF] Found not standard event log");
                        }
                    });

                    updatePositionSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        var decodedUpdatePosition = Event<UpdatePositionDTO>.DecodeEvent(log);
                        if (decodedUpdatePosition != null)
                        {
                            Console.WriteLine("[DEV-INF] Decoding a update position event ...");
                        }
                        else
                        {
                            Console.WriteLine("[DEV-INF] Found not standard event log");
                        }
                    });

                    closePositionSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        var decodedClosePosition = Event<ClosePositionDTO>.DecodeEvent(log);
                        if (decodedClosePosition != null)
                        {
                            Console.WriteLine("[DEV-INF] Decoding a close position event ...");
                        }
                        else
                        {
                            Console.WriteLine("[DEV-INF] Found not standard event log");
                        }
                    });

                    liquidatePositionSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async log =>
                    {
                        var decodedLiquidatePosition = Event<LiquidatePositionDTO>.DecodeEvent(log);
                        if (decodedLiquidatePosition != null)
                        {
                            Console.WriteLine("[DEV-INF] Decoding a liquidate position event ...");
                        }
                        else
                        {
                            Console.WriteLine("[DEV-INF] Found not standard event log");
                        }
                    });

                    await client.StartAsync();
                    await swapSubscription.SubscribeAsync(swapFilterInput);
                    await reporterAddedSubscription.SubscribeAsync(reporterAddedFilterInput);
                    await reporterRemovedSubscription.SubscribeAsync(reporterRemovedFilterInput);
                    await reporterPostedSubscription.SubscribeAsync(reporterPostedFilterInput);
                    await increasePositionSubscription.SubscribeAsync(increasePositionFilterInput);
                    await decreasePositionSubscription.SubscribeAsync(decreasePositionFilterInput);
                    await updatePositionSubscription.SubscribeAsync(updatePositionFilterInput);
                    await closePositionSubscription.SubscribeAsync(closePositionFilterInput);
                    await liquidatePositionSubscription.SubscribeAsync(liquidatePositionFilterInput);
                    while (true)
                    {
                        Console.WriteLine("[DEV-INF] Client state: " + client.WebSocketState);
                        if (client.WebSocketState == System.Net.WebSockets.WebSocketState.Aborted || client.WebSocketState == System.Net.WebSockets.WebSocketState.Closed)
                        {
                            //restart client
                            Console.WriteLine($"[DEV-INF] Client {client.WebSocketState}, restarting ...");
                            await swapSubscription.UnsubscribeAsync();
                            await reporterAddedSubscription.UnsubscribeAsync();
                            await reporterRemovedSubscription.UnsubscribeAsync();
                            await reporterPostedSubscription.UnsubscribeAsync();
                            await increasePositionSubscription.UnsubscribeAsync();
                            await decreasePositionSubscription.UnsubscribeAsync();
                            await updatePositionSubscription.UnsubscribeAsync();
                            await closePositionSubscription.UnsubscribeAsync();
                            await liquidatePositionSubscription.UnsubscribeAsync();
                            await client.StopAsync();
                            await client.StartAsync();
                            await swapSubscription.SubscribeAsync(swapFilterInput);
                            await reporterAddedSubscription.SubscribeAsync(reporterAddedFilterInput);
                            await reporterRemovedSubscription.SubscribeAsync(reporterRemovedFilterInput);
                            await reporterPostedSubscription.SubscribeAsync(reporterPostedFilterInput);
                            await increasePositionSubscription.SubscribeAsync(increasePositionFilterInput);
                            await decreasePositionSubscription.SubscribeAsync(decreasePositionFilterInput);
                            await updatePositionSubscription.SubscribeAsync(updatePositionFilterInput);
                            await closePositionSubscription.SubscribeAsync(closePositionFilterInput);
                            await liquidatePositionSubscription.SubscribeAsync(liquidatePositionFilterInput);
                        }
                        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEV-ERR] WebSocket error: {ex.Message}, switching ...");
                }
            }
        }

        private string HandleWebsocketString()
        {
            string wss = _wss[currentWssIndex];
            currentWssIndex = (currentWssIndex + 1) % _wss.Count;
            return wss;
        }
    }
}