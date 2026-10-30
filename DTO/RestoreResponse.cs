namespace ConstellationRestore.Dtos;

public class RestoreResponse
{
    public string MissingSymbols { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}