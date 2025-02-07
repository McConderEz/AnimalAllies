using System.Text;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.DTOs;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CheckIfPetByBreedIdExist;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Volunteer.Application.VolunteerManagement.Commands.CheckIfPetBySpeciesIdExist;

public class CheckIfPetBySpeciesIdExistHandler: IQueryHandler<bool, CheckIfPetBySpeciesIdExistQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<CheckIfPetBySpeciesIdExistHandler> _logger;

    public CheckIfPetBySpeciesIdExistHandler(
        [FromKeyedServices(Constraints.Context.PetManagement)]ISqlConnectionFactory sqlConnectionFactory,
        ILogger<CheckIfPetBySpeciesIdExistHandler> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(
        CheckIfPetBySpeciesIdExistQuery query ,
        CancellationToken cancellationToken = default)
    {
        
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        
        parameters.Add("@SpeciesId", query.Id);
        var sql = new StringBuilder("""
                                    select 
                                        id
                                        from volunteers.pets
                                        where species_id = @SpeciesId and
                                              is_deleted = false
                                        limit 1
                                    """);
        
        var pets = 
            (await connection.QueryAsync<PetDto>(
                sql.ToString(),
                param: parameters)).ToList();
        
        _logger.LogInformation("Get pets with species id {speciesId}", query.Id);

        if (pets.Any())
            return true;

        return false;
    }
}