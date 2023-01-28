﻿using OverCloudAirways.BookingService.Domain.Aircrafts;
using OverCloudAirways.BuildingBlocks.Application.Models;

namespace OverCloudAirways.BookingService.Application.Aircrafts.Commands.ProjectReadModel;

internal record AircraftReadModel(
    Guid Id,
    string Type,
    string Manufacturer,
    string Model,
    int SeatingCapacity,
    int EconomyCostPerKM,
    int FirstClassCostPerKM,
    int Range,
    int CruisingAltitude,
    int MaxTakeoffWeight,
    int Length,
    int Wingspan,
    int Height,
    IReadOnlyCollection<Engine> Engines)
    : ReadModel(Id.ToString(), Id.ToString());