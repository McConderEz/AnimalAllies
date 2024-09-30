using System.Text;
using System.Text.Json;
using AnimalAllies.Application.Abstractions;
using AnimalAllies.Application.Contracts.DTOs;
using AnimalAllies.Application.Contracts.DTOs.ValueObjects;
using AnimalAllies.Application.Database;
using AnimalAllies.Application.Extension;
using AnimalAllies.Domain.Shared;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Application.Features.Volunteer.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetPetByIdHandler> _logger;
    private readonly IValidator<GetPetByIdQuery> _validator;

    public GetPetByIdHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<GetPetByIdHandler> logger,
        IValidator<GetPetByIdQuery> validator)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<PetDto>> Handle(GetPetByIdQuery query, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();
        
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        
        parameters.Add("@PetId", query.PetId);

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
                                        health_information,
                                        pet_details_description,
                                        requisites,
                                        pet_photos
                                        from pets
                                    """);
        
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
        
        var result = pets.FirstOrDefault();

        if (result is null) 
            return Errors.General.NotFound();
        
        _logger.LogInformation("Get pet with id {petId}", query.PetId);
        
        return result;
    }
}