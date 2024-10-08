using System.Text;
using System.Text.Json;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Shared;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Queries.GetPetsByBreedId;

public class GetPetsByBreedIdHandler: IQueryHandler<List<PetDto>, GetPetsByBreedIdQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetPetsByBreedIdHandler> _logger;
    private readonly IValidator<GetPetsByBreedIdQuery> _validator;

    public GetPetsByBreedIdHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<GetPetsByBreedIdHandler> logger,
        IValidator<GetPetsByBreedIdQuery> validator)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<List<PetDto>>> Handle(
        GetPetsByBreedIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(query, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();
        
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        
        parameters.Add("@BreedId", query.BreedId);

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
                                        where breed_id = @BreedId
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
        
        
        _logger.LogInformation("Get pets with breed id {breedId}", query.BreedId);

        return pets.ToList();
    }
}