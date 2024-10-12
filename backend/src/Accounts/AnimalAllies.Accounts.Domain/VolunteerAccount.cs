
namespace AnimalAllies.Accounts.Domain;

public class VolunteerAccount
{
    public FullName FullName { get; set; }
    public int Experience { get; set; }
    public List<Certificate> Certificates { get; set; } = [];
    public List<Requisite> Requisites { get; set; } = [];
    public Guid UserId { get; set; }
    public User User { get; set; }
}