namespace AvitoAdsParser.Parsers;

public static class EnvParser
{
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);

        foreach (var line in File.ReadAllLines(filePath))
        {
            var parts = line.Split('=');
            if (parts.Length < 2)
            {
                continue;
            }

            if (parts.Length > 2)
            {
                parts[1] = string.Join('=', parts.Skip(1));
            }

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}
