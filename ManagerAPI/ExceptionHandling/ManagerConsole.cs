namespace ManagerAPI.ExceptionHandling;

public static class ManagerConsole
{
    public static void WriteException(String message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = oldColor;
    }
}
