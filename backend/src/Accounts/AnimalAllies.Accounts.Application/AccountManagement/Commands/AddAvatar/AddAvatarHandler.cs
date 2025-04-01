using AnimalAllies.Accounts.Contracts.Responses;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Abstractions;
using AnimalAllies.Core.Database;
using AnimalAllies.Core.Extension;
using AnimalAllies.SharedKernel.Constraints;
using AnimalAllies.SharedKernel.Shared;
using AnimalAllies.SharedKernel.Shared.Errors;
using AnimalAllies.SharedKernel.Shared.ValueObjects;
using FileService.Communication;
using FileService.Contract.Requests;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AnimalAllies.Accounts.Application.AccountManagement.Commands.AddAvatar;

public class AddAvatarHandler: ICommandHandler<AddAvatarCommand, AddAvatarResponse>
{
    private readonly ILogger<AddAvatarHandler> _logger;
    private readonly IValidator<AddAvatarCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly FileHttpClient _fileHttpClient;

    public AddAvatarHandler(
        ILogger<AddAvatarHandler> logger, 
        IValidator<AddAvatarCommand> validator,
        [FromKeyedServices(Constraints.Context.Accounts)]IUnitOfWork unitOfWork,
        UserManager<User> userManager, 
        FileHttpClient fileHttpClient)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _fileHttpClient = fileHttpClient;
    }

    public async Task<Result<AddAvatarResponse>> Handle(
        AddAvatarCommand command, CancellationToken cancellationToken = default)
    {
        var validatorResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validatorResult.IsValid)
            return validatorResult.ToErrorList();

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
        if (user is null)
            return Errors.General.NotFound();

        var request = new UploadPresignedUrlRequest(
            command.UploadFileDto.BucketName,
            command.UploadFileDto.FileName,
            command.UploadFileDto.ContentType);

        var response = await _fileHttpClient.GetUploadPresignedUrlAsync(request, cancellationToken);
        if (response is null)
            return Errors.General.Null("response from file service is null");

        user.Photo = response.FileId + response.Extension;

        var addAvatarResponse = new AddAvatarResponse(response.UploadUrl);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Added avatar to user with id {id}", command.UserId);
        
        return addAvatarResponse;
    }
}