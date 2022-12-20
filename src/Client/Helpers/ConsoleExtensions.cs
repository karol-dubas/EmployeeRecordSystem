using Newtonsoft.Json;

namespace EmployeeRecordSystem.Client.Helpers;

public static class DebugConsole
{
    public static void WriteLineAsJson<TContent>(TContent content, string comment = null)
    {
        if (comment is not null)
            Console.WriteLine(comment);
        
        string json = JsonConvert.SerializeObject(content, Formatting.Indented);
        Console.WriteLine(json);
    }
}
