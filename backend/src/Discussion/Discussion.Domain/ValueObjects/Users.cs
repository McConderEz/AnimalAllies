using AnimalAllies.SharedKernel.Shared;

namespace Discussion.Domain.ValueObjects;

public class Users: ValueObject
{
    public Guid FirstUserId { get; }
    public Guid SecondUserId { get; }
    
    private Users(){}

    private Users(Guid firstUserId, Guid secondUserId)
    {
        FirstUserId = firstUserId;
        SecondUserId = secondUserId;
    }

    public static Result<Users> Create(Guid firstUserId, Guid secondUserId)
    {
        if (firstUserId == Guid.Empty || secondUserId == Guid.Empty)
            return Errors.General.Null("one of users ids");

        return new Users(firstUserId, secondUserId);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstUserId;
        yield return SecondUserId;
    }
}