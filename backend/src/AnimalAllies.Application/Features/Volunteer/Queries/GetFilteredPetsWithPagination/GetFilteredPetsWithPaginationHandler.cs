using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Application.Models;
using AnimalAllies.Domain.Models.Volunteer.Pet;
using AnimalAllies.Domain.Shared;
using Dapper;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetFilteredPetsWithPagination;

public class GetFilteredPetsWithPaginationHandler : IQueryHandler<PagedList<PetDto>, GetFilteredPetsWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetFilteredPetsWithPaginationHandler> _logger;

    public GetFilteredPetsWithPaginationHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<GetFilteredPetsWithPaginationHandler> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<Result<PagedList<PetDto>>> Handle(
        GetFilteredPetsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        
        var sql = new StringBuilder("""
                                    select 
                                        id,
                                        volunteer_id,
                                        name,
                                        city,
                                        state,
                                        street,
                                        zip_code,
                                        breed_id,
                                        species_id,
                                        help_status,
                                        phone_number,
                                        birth_date,
                                        color,
                                        height,
                                        weight,
                                        is_castrated,
                                        is_vaccinated,
                                        position,
                                        requisites,
                                        pet_photos
                                        from pets
                                    """);
        
        bool hasWhereClause = false;
        
        var stringProperties = new Dictionary<string, string>
        {
            { "name", query.Name },
            { "city", query.City },
            { "state", query.State },
            { "street", query.Street },
            { "zip_code", query.ZipCode },
            { "breed_id", query.BreedId.ToString() },
            { "species_id", query.SpeciesId.ToString() },
            { "help_status", query.HelpStatus },
            { "color", query.Color },
        };

        sql.ApplyFilterByString(ref hasWhereClause, stringProperties);
        
        FilterByValue(ref hasWhereClause,query, sql);
        
        sql.ApplySorting(query.SortBy,query.SortDirection);
        
        sql.ApplyPagination(query.Page,query.PageSize);
        
        var pets = 
            await connection.QueryAsync<PetDto, string, string, PetDto>(
                sql.ToString(),
                (pet, jsonRequisites, jsonPetPhotos) =>
                {
                    var requisites = JsonSerializer.Deserialize<RequisiteDto[]>(jsonRequisites) ?? [];
                    pet.Requisites = requisites;

                    var petPhotoDtos = JsonSerializer.Deserialize<PetPhotoDto[]>(jsonPetPhotos) ?? [];
                    pet.PetPhotos = petPhotoDtos;
                    
                    return pet;
                },
                splitOn:"requisites, pet_photos",
                param: parameters);
        
        
        _logger.LogInformation("Get pets with pagination Page: {Page}, PageSize: {PageSize}",
            query.Page, query.PageSize);

        var petDtos = pets.ToList();
        
        return new PagedList<PetDto>
        {
            Items = petDtos,
            PageSize = query.PageSize,
            Page = query.Page,
            TotalCount = petDtos.Count()
        };
    }

    private static void FilterByValue(ref bool hasWhereClause, GetFilteredPetsWithPaginationQuery query, StringBuilder sql)
    {
        switch (query)
        {
            case { PositionFrom: not null, PositionTo: not null }:
                sql.ApplyBetweenFilter(ref hasWhereClause,"position", (int)query.PositionFrom, (int)query.PositionTo);
                break;
            case {PositionFrom: not null, PositionTo: null}:
                sql.ApplyFilterByValueFrom(ref hasWhereClause,"position", (int)query.PositionFrom);
                break;
            case {PositionFrom: null, PositionTo: not null}:
                sql.ApplyFilterByValueTo<int>(ref hasWhereClause,"position", (int)query.PositionTo);
                break;
        }
        
        switch (query)
        {
            case { WeightFrom: not null, WeightTo: not null }:
                sql.ApplyBetweenFilter(ref hasWhereClause,"weight", (int)query.WeightFrom, (int)query.WeightTo);
                break;
            case {WeightFrom: not null, WeightTo: null}:
                sql.ApplyFilterByValueFrom(ref hasWhereClause,"weight", (int)query.WeightFrom);
                break;
            case {WeightFrom: null, WeightTo: not null}:
                sql.ApplyFilterByValueTo(ref hasWhereClause,"weight", (int)query.WeightTo);
                break;
        }
        
        switch (query)
        {
            case { HeightFrom: not null, HeightTo: not null }:
                sql.ApplyBetweenFilter(ref hasWhereClause,"height", (int)query.HeightFrom, (int)query.HeightTo);
                break;
            case {HeightFrom: not null, HeightTo: null}:
                sql.ApplyFilterByValueFrom(ref hasWhereClause,"height", (int)query.HeightFrom);
                break;
            case {HeightFrom: null, HeightTo: not null}:
                sql.ApplyFilterByValueTo(ref hasWhereClause,"height", (int)query.HeightTo);
                break;
        }

        switch (query)
        {
            case { BirthDateFrom: not null, BirthDateTo: not null }:
                sql.ApplyBetweenFilter(ref hasWhereClause,"birth_date", DateOnly.FromDateTime((DateTime)query.BirthDateFrom),
                    DateOnly.FromDateTime((DateTime)query.BirthDateTo));
                break;
            case {BirthDateFrom: not null, BirthDateTo: null}:
                sql.ApplyFilterByValueFrom(ref hasWhereClause,"birth_date",
                    DateOnly.FromDateTime((DateTime)query.BirthDateFrom));
                break;
            case {BirthDateFrom: null, BirthDateTo: not null}:
                sql.ApplyFilterByValueTo(ref hasWhereClause,"birth_date", DateOnly.FromDateTime((DateTime)query.BirthDateTo));
                break;
        }
        
        if(query.IsCastrated is not null)
            sql.ApplyFilterByEqualsValue(ref hasWhereClause,"is_castrated", query.IsCastrated);
        
        if(query.IsVaccinated is not null)
            sql.ApplyFilterByEqualsValue(ref hasWhereClause,"is_vaccinated", query.IsVaccinated);
    }
}