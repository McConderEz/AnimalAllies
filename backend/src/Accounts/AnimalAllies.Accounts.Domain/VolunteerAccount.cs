
using AnimalAllies.SharedKernel.Shared.ValueObjects;

namespace AnimalAllies.Accounts.Domain;

public class VolunteerAccount
{
    public static readonly string Volunteer = nameof(Volunteer);
    private VolunteerAccount(){}

    public VolunteerAccount(FullName fullName,int experience, User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Experience = experience;
        User = user;
    }
    public Guid Id { get; set; }
    public FullName FullName { get; set; }
    public int Experience { get; set; }
    public List<Certificate> Certificates { get; set; } = [];
    public List<Requisite> Requisites { get; set; } = [];
    public Guid UserId { get; set; }
    public User User { get; set; }
}