namespace AnimalAllies.Accounts.Domain;

public class Certificate
{
    public string Title { get; set; } = string.Empty;
    public string IssuingOrganization { get; set; } = string.Empty;
    public DateOnly IssueDate { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public string Description { get; set; } = String.Empty;
}