namespace TextStorage;

public class AppSettings
{
    public required ConnectionStrings ConnectionStrings { get; set; }
}


public class ConnectionStrings
{
    public required string Master1 { get; set; }
    public required string Master2 { get; set; }
    public required string Master3 { get; set; }
}