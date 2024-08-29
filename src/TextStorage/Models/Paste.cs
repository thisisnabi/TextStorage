namespace TextStorage.Models;

public class Paste
{
    public long Id { get; set; }

    public required string Content { get; set; }

    public DateTime ExpireOn { get; set; }

    public required string ShortenCode { get; set; }

    public string? Password { get; set; }

    public static Paste Create(string content, string code, string password, DateTime? expireOn = default)
    {
        expireOn ??= DateTime.Now.AddDays(7);
        return new Paste
        {
            Content = content,
            ShortenCode = code,
            ExpireOn = expireOn.Value,
            Password  = password
        };
    }

}
