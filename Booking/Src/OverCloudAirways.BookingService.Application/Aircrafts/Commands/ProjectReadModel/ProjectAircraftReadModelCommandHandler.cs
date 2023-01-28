﻿using OverCloudAirways.BookingService.Domain.Aircrafts;
using OverCloudAirways.BuildingBlocks.Application.Commands;
using OverCloudAirways.BuildingBlocks.Domain.Abstractions;
using OverCloudAirways.BuildingBlocks.Infrastructure.CosmosDB;

namespace OverCloudAirways.BookingService.Application.Aircrafts.Commands.ProjectReadModel;

internal class ProjectAircraftReadModelCommandHandler : CommandHandler<ProjectAircraftReadModelCommand>
{
    private readonly IAggregateRepository _aggregateRepository;
    private readonly ICosmosManager _cosmosManager;

    public ProjectAircraftReadModelCommandHandler(
        IAggregateRepository aggregateRepository,
        ICosmosManager cosmosManager)
    {
        _aggregateRepository = aggregateRepository;
        _cosmosManager = cosmosManager;
    }

    public override async Task HandleAsync(ProjectAircraftReadModelCommand command, CancellationToken cancellationToken)
    {
        var aggregate = await _aggregateRepository.LoadAsync<Aircraft, AircraftId>(command.AircraftId);
        var readmodel = new AircraftReadModel(
            aggregate.Id.Value,
            aggregate.Type,
            aggregate.Manufacturer,
            aggregate.Model,
            aggregate.SeatingCapacity,
            aggregate.EconomyCostPerKM,
            aggregate.FirstClassCostPerKM,
            aggregate.Range,
            aggregate.CruisingAltitude,
            aggregate.MaxTakeoffWeight,
            aggregate.Length,
            aggregate.Wingspan,
            aggregate.Height,
            aggregate.Engines);

        await _cosmosManager.UpsertAsync(ContainersConstants.ReadModels, readmodel);
    }
}
