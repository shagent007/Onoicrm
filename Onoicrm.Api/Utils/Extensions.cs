namespace Onoicrm.Api.Utils;

public static class Extensions
{
    public static string GetEnvConnectionString(this IConfiguration configuration, string envFilePath)
    {
        //comment
        var postgresHost = GetEnvironmentVariable("POSTGRES_HOST",envFilePath);
        var postgresDb = GetEnvironmentVariable("POSTGRES_DB",envFilePath);
        var postgresPort = GetEnvironmentVariable("POSTGRES_PORT",envFilePath);
        var postgresUser = GetEnvironmentVariable("POSTGRES_USER",envFilePath);
        var postgresPassword = GetEnvironmentVariable("POSTGRES_PASSWORD",envFilePath);

        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Host={POSTGRES_HOST};Port={POSTGRES_PORT};Database={POSTGRES_DB};Username={POSTGRES_USER};Password={POSTGRES_PASSWORD};Include Error Detail=true";
        connectionString = connectionString    
            .Replace("{POSTGRES_HOST}", postgresHost)    
            .Replace("{POSTGRES_DB}", postgresDb)
            .Replace("{POSTGRES_PORT}", postgresPort)
            .Replace("{POSTGRES_USER}", postgresUser)
            .Replace("{POSTGRES_PASSWORD}", postgresPassword);


        return connectionString;
    }


    public static string GetEnvironmentVariable(string variableName, string envFilePath)
    {
        var value = Environment.GetEnvironmentVariable(variableName);

        if (value != null) return value;
        value = ReadValueFromEnvFile(envFilePath, variableName);
        return value;
    }

    private static string ReadValueFromEnvFile(string filePath, string variableName)
    {
        var value = "";

        try
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    var parts = line.Split('=');
                    if (parts.Length != 2 || parts[0].Trim() != variableName) continue;
                    value = parts[1].Trim();
                    break;
                }
            }
            else
            {
                Console.WriteLine($"Файл {filePath} не найден.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка чтения файла {filePath}: {ex.Message}");
        }

        return value;
    }

    
    public static IEnumerable<tsource> Groupmaxby<tsource, tkey, tvalue>(
        this IEnumerable<tsource> source,
        Func<tsource, tkey> keyselector,
        Func<tsource, tvalue> valueselector,
        IEqualityComparer<tkey> keycomparer = default,
        IComparer<tvalue> valuecomparer = default)
    {
        var dictionary = new Dictionary<tkey, (tsource item, tvalue value)>(keycomparer);
        foreach (var item in source)
        {
            var key = keyselector(item);
            var value = valueselector(item);
            if (dictionary.TryGetValue(key, out var existing) &&
                valuecomparer.Compare(existing.value, value) >= 0) continue;
            dictionary[key] = (item, value);
        }
        foreach (var entry in dictionary.Values)
            yield return entry.item;
    }


}