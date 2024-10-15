using System.Text;
using System.Text.Json;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs;
using AnimalAllies.Core.DTOs.ValueObjects;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Queries.GetPetsByBreedId;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CheckIfPetByBreedIdExist;

public class CheckIfPetByBreedIdExistHandler: IQueryHandler<bool, CheckIfPetByBreedIdExistQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<CheckIfPetByBreedIdExistHandler> _logger;

    public CheckIfPetByBreedIdExistHandler(
        [FromKeyedServices(Constraints.Context.PetManagement)]ISqlConnectionFactory sqlConnectionFactory,
        ILogger<CheckIfPetByBreedIdExistHandler> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        CheckIfPetByBreedIdExistQuery query ,
        CancellationToken cancellationToken = default)
    {
        
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        
        parameters.Add("@BreedId", query.Id);
        var sql = new StringBuilder("""
                                    select 
                                        id
                                        from volunteers.pets
                                        where breed_id = @BreedId and
                                              is_deleted = false
                                        limit 1
                                    """);
        
        var pets = 
            (await connection.QueryAsync<PetDto>(
                sql.ToString(),
                param: parameters)).ToList();
        
        _logger.LogInformation("Get pets with breed id {breedId}", query.Id);

        if (pets.Any())
            return true;

        return false;
    }
}