using Newtonsoft.Json;

namespace EmployeeRecordSystem.Client.Helpers;

public static class DebugConsole
{
    public static void WriteLineAsJson<TContent>(TContent content, string comment = null)
    {
        string json = JsonConvert.SerializeObject(content, Formatting.Indented);

        if (comment is not null)
            Console.WriteLine(comment);

        Console.WriteLine(json);
    }
}
