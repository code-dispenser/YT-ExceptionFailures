using System.Text.Json.Serialization;

namespace ExceptionFailures.ConsoleClient.Common.Models;
public class ProblemDetails
{
    public required string Type     { get; set; }
    public required string Title    { get; set; }
    public int? Status              { get; set; }
    public required string Detail   { get; set; }
    public required string Instance { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object?> Extensions { get; set; } = [];

    [JsonConstructor]
    private ProblemDetails() { }

    public ProblemDetails(string type, string title, int? status, string detail, string instance, Dictionary<string, object?> extensions)
    {
        Type        = type;
        Title       = title;
        Status      = status;
        Detail      = detail;
        Instance    = instance;
        Extensions  = extensions;
    }
}
