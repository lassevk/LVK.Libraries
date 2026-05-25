namespace LVK.Hosting.ConsoleApplications;

public class ConsoleLine
{
    public void Set(string value)
    {
        if (Current == value)
            return;

        if (value == "")
            System.Console.Out.ShowCursor();
        else
            System.Console.Out.HideCursor();

        System.Console.Out.MoveToStartOfLine().Write(value);
        System.Console.Out.ClearToEndOfLine();

        Current = value;
    }

    public void Clear() => Set("");

    public string Current { get; private set; } = "";
}