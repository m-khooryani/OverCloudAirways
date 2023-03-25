﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OverCloudAirways.BuildingBlocks.Application.Commands;
using OverCloudAirways.BuildingBlocks.Domain.Abstractions;
using OverCloudAirways.BuildingBlocks.Domain.Models;
using OverCloudAirways.BuildingBlocks.Domain.Utilities;

namespace OverCloudAirways.BuildingBlocks.Infrastructure.RequestProcessing;

internal class CommandsScheduler : ICommandsScheduler
{
    private readonly ILogger _logger;
    private readonly IUserAccessor _userAccessor;
    private readonly IOutboxRepository _repository;

    public CommandsScheduler(
        ILogger logger,
        IUserAccessor userAccessor,
        IOutboxRepository repository)
    {
        _logger = logger;
        _userAccessor = userAccessor;
        _repository = repository;
    }

    public async Task EnqueueAsync(ICommand command)
    {
        var outboxMessage = OutboxMessage.Create(
            Clock.Now,
            command,
            _userAccessor.UserId,
            _userAccessor.TcpConnectionId);

        await AddOutboxMessageAsync(outboxMessage);
    }

    public async Task EnqueueAsync<TResult>(ICommand<TResult> command)
    {
        var outboxMessage = OutboxMessage.Create(
            Clock.Now,
            command,
            _userAccessor.UserId,
            _userAccessor.TcpConnectionId);

        await AddOutboxMessageAsync(outboxMessage);
    }

    public async Task EnqueuePublishingEventAsync(IntegrationEvent integrationEvent)
    {
        var outboxMessage = OutboxMessage.Create(
            Clock.Now,
            integrationEvent,
            _userAccessor.UserId,
            _userAccessor.TcpConnectionId);

        await AddOutboxMessageAsync(outboxMessage);
    }

    public async Task ScheduleAsync(ICommand command, DateTimeOffset date)
    {
        var outboxMessage = OutboxMessage.CreateDelayed(
            Clock.Now,
            command,
            _userAccessor.UserId,
            _userAccessor.TcpConnectionId,
            date);

        await AddOutboxMessageAsync(outboxMessage);
    }

    private async Task AddOutboxMessageAsync(OutboxMessage outboxMessage)
    {
        var json = JsonConvert.SerializeObject(outboxMessage, Formatting.Indented);
        _logger.LogInformation("OutboxMessage added: {json}", json);

        await _repository.AddAsync(outboxMessage);
    }
}
