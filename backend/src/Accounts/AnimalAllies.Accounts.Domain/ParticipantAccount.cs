namespace AnimalAllies.Accounts.Domain;

public class ParticipantAccount
{
    public Guid Id { get; set; }
    public FullName FullName { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}