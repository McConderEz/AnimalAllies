using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnimalAllies.Accounts.Application;
using AnimalAllies.Accounts.Application.Models;
using AnimalAllies.Accounts.Domain;
using AnimalAllies.Core.Models;
using AnimalAllies.Core.Options;
using AnimalAllies.Framework;
using AnimalAllies.SharedKernel.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AnimalAllies.Accounts.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly RefreshSessionOptions _refreshSessionOptions;
    private readonly JwtOptions _jwtOptions;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly AccountsDbContext _accountsDbContext;

    public JwtTokenProvider(
        IOptions<JwtOptions> options,
        IDateTimeProvider dateTimeProvider,
        IOptions<RefreshSessionOptions> refreshSessionOptions,
        AccountsDbContext accountsDbContext)
    {
        _dateTimeProvider = dateTimeProvider;
        _accountsDbContext = accountsDbContext;
        _refreshSessionOptions = refreshSessionOptions.Value;
        _jwtOptions = options.Value;
    }

    public JwtTokenResult GenerateAccessToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roleClaims = user.Roles
            .Select(r => new Claim(CustomClaims.Role, r.Name ?? string.Empty));

        var jti = Guid.NewGuid();
        
        var claims = new[]
        {
            new Claim(CustomClaims.Id, user.Id.ToString()),
            new Claim(CustomClaims.Email, user.Email!),
            new Claim(CustomClaims.Username, user.UserName!),
            new Claim(CustomClaims.Jti, jti.ToString())
        };

        claims = claims.Concat(roleClaims).ToArray();
        
        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtOptions.ExpiredMinutesTime)),
            signingCredentials: signingCredentials,
            claims: claims);


        var jwtStringToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new JwtTokenResult(jwtStringToken, jti);
    }

    public async Task<Guid> GenerateRefreshToken(
        User user,
        Guid accessTokenJti,
        CancellationToken cancellationToken = default)
    {
        var refreshSession = new RefreshSession()
        {
            User = user,
            CreatedAt = _dateTimeProvider.UtcNow,
            Jti = accessTokenJti,
            ExpiresIn = _dateTimeProvider.UtcNow.AddDays(_refreshSessionOptions.ExpiredDaysTime),
            RefreshToken = Guid.NewGuid()
        };

        await _accountsDbContext.RefreshSessions.AddAsync(refreshSession, cancellationToken);
        await _accountsDbContext.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    }
    
    public async Task<Result<IReadOnlyList<Claim>>> GetUserClaims(
        string jwtToken,
        CancellationToken cancellationToken = default)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        var validationParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions);

        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);
        if (!validationResult.IsValid)
            return Errors.Tokens.InvalidToken();

        return validationResult.ClaimsIdentity.Claims.ToList();
    }
}