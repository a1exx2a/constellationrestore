namespace ConstellationRestore.Services;

public class ConstellationService : IConstellationService
{
    public string GetMissingSymbols(string reference, string damaged)
    {
        var missing = new List<char>(reference.Length);

        for (int i = 0; i < reference.Length; i++)
        {
            char refChar = reference[i];
            bool found = false;

            for (int j = 0; j < damaged.Length; j++)
            {
                if (damaged[j] == refChar)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                missing.Add(refChar);
            }
        }

        char[] result = missing.ToArray();
        Array.Sort(result);
        return new string(result);
    }
}