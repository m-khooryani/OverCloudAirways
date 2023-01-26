﻿using DArch.Application.Configuration.Commands;
using OverCloudAirways.BuildingBlocks.Application.DomainEventPolicies;
using OverCloudAirways.IdentityService.Application.Users.Commands.UpdateReadModel;
using OverCloudAirways.IdentityService.Domain.Users.Events;

namespace OverCloudAirways.IdentityService.Application.Users.Policies.Registered;

internal class UpdateUserReadModelPolicyHandler : IDomainPolicyHandler<UserRegisteredPolicy, UserRegisteredDomainEvent>
{
    private readonly ICommandsScheduler _commandsScheduler;

    public UpdateUserReadModelPolicyHandler(ICommandsScheduler commandsScheduler)
    {
        _commandsScheduler = commandsScheduler;
    }

    public async Task Handle(UserRegisteredPolicy notification, CancellationToken cancellationToken)
    {
        var command = new ProjectUserReadModelCommand(notification.DomainEvent.UserId);
        await _commandsScheduler.EnqueueAsync(command);
    }
}
