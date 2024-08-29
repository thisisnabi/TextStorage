namespace TextStorage;

public class TenantPrincipal
{
    public int Id { get; set; }
    public required string ConnectionString { get; set; }

    public char Predicate { get; set; }

}
