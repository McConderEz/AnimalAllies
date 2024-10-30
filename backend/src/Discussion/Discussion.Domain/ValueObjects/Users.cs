﻿using AnimalAllies.SharedKernel.Shared;

namespace Discussion.Domain.ValueObjects;

public class Users: ValueObject
{
    public Guid FirstMember { get; }
    public Guid SecondMember{ get; }
    
    private Users(){}

    private Users(Guid firstMember, Guid secondMember)
    {
        FirstMember = firstMember;
        SecondMember = secondMember;
    }

    public static Result<Users> Create(Guid firstMember, Guid secondMember)
    {
        if (firstMember == Guid.Empty || secondMember == Guid.Empty)
            return Errors.General.Null("one of users ids");

        return new Users(firstMember, secondMember);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstMember;
        yield return SecondMember;
    }
}