namespace ConstellationRestore.Services;

public interface IConstellationService
{
    string GetMissingSymbols(string reference, string damaged);
}