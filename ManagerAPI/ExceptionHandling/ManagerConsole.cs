namespace ManagerAPI.ExceptionHandling;

public static class ManagerConsole
{
    public static void WriteLayerMessage()
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"API Layer Message...");
        Console.ForegroundColor = oldColor;
    }

    public static void WriteInformation(string methodDomain, string customMessage)
    {
        WriteLayerMessage();
        var oldColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        string message = BuildMessage(methodDomain, customMessage);
        Console.WriteLine($"[INFO]\t{message}");

        Console.ForegroundColor = oldColor;
    }

    public static void WriteWarning(string methodDomain, string customMessage)
    {
        WriteLayerMessage();
        var oldColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        string message = BuildMessage(methodDomain, customMessage);
        Console.WriteLine($"[WARN]\t{message}");

        Console.ForegroundColor = oldColor;
    }

    public static void WriteException(string methodDomain, string customMessage, Exception? ex = null)
    {
        WriteLayerMessage();
        var oldColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Red;
        string exceptionMessage = BuildExceptionMessage(customMessage, ex);
        string message = BuildMessage(methodDomain, exceptionMessage);
        Console.WriteLine($"[ERROR]\t{message}");

        Console.ForegroundColor = oldColor;
    }

    /// <summary>
    /// DO NOT MODIFY - Concatenates the methodDomain and the Message and adds tabs to keep it organized and viewer friendly
    /// </summary>
    /// <param name="methodDomain"></param>
    /// <param name="message"></param>
    /// <returns>A Message ready for the ManagerConsole to Write</returns>
    public static string BuildMessage(string methodDomain, string message)
    {
        return $"Invoker:{methodDomain} at {DateTime.UtcNow}\n{message}".Replace("\n", "\n\t");
    }

    /// <summary>
    /// DO NOT MODIFY - Concatenates a customMessage with the Exception StackTrace
    /// </summary>
    /// <param name="customMessage"></param>
    /// <param name="ex"></param>
    /// <returns>An Exception Message ready for the ManagerConsole to Write</returns>
    public static string BuildExceptionMessage(string customMessage, Exception? ex = null)
    {
        string exception = ex == null ? string.Empty : $"\nReason: {ex}";
        return $"{customMessage}{exception}";
    }
}
