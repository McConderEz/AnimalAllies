﻿using AnimalAllies.Application.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AnimalAllies.Application.Database;

public interface IReadDbContext
{
    IQueryable<VolunteerDto> Volunteers { get; }
}