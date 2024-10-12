namespace AnimalAllies.Accounts.Domain;

public class ParticipantAccount
{
    public FullName FullName { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}