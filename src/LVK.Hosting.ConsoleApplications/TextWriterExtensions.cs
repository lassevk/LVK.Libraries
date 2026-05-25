namespace LVK.Hosting.ConsoleApplications;

public static class TextWriterExtensions
{
    public static TextWriter Move(this TextWriter target, int deltaColumn, int deltaRow)
    {
        ArgumentNullException.ThrowIfNull(target);

        if (deltaColumn == 0 && deltaRow == 0)
        {
            return target;
        }

        if (deltaColumn < 0)
        {
            target.MoveLeft(-deltaColumn);
        }
        else if (deltaColumn > 0)
        {
            target.MoveRight(deltaColumn);
        }

        if (deltaRow < 0)
        {
            target.MoveUp(-deltaRow);
        }
        else if (deltaRow > 0)
        {
            target.MoveDown(deltaRow);
        }

        return target;
    }

    public static TextWriter MoveUp(this TextWriter target, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

        if (amount == 0)
        {
            return target;
        }

        target.Write($"\u001b[{amount}A");

        return target;
    }

    public static TextWriter MoveDown(this TextWriter target, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

        if (amount == 0)
        {
            return target;
        }

        target.Write($"\u001b[{amount}B");

        return target;
    }

    public static TextWriter MoveRight(this TextWriter target, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

        if (amount == 0)
        {
            return target;
        }

        target.Write($"\u001b[{amount}C");

        return target;
    }

    public static TextWriter MoveLeft(this TextWriter target, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

        if (amount == 0)
        {
            return target;
        }

        target.Write($"\u001b[{amount}D");

        return target;
    }

    public static TextWriter MoveToStartOfLineUpwards(this TextWriter target, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

        if (amount == 0)
        {
            return target;
        }

        target.Write($"\u001b[{amount}F");

        return target;
    }

    public static TextWriter MoveToStartOfLineDownwards(this TextWriter target, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

        if (amount == 0)
        {
            return target;
        }

        target.Write($"\u001b[{amount}E");

        return target;
    }

    public static TextWriter MoveToColumn(this TextWriter target, int column)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(column, 0);

        if (column == 0)
        {
            return target;
        }

        target.Write($"\u001b[{column}G");

        return target;
    }

    public static TextWriter MoveToStartOfLine(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write($"\u001b[1G");

        return target;
    }

    public static TextWriter MoveTo(this TextWriter target, int column, int row)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(column, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(row, 1);

        target.Write($"\u001b[{column};{row}H");

        return target;
    }

    public static TextWriter ClearToEndOfLine(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[K");

        return target;
    }

    public static TextWriter ClearToStartOfLine(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[1K");

        return target;
    }

    public static TextWriter ClearEntireLine(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[2K");

        return target;
    }


    public static TextWriter ClearToEndOfScreen(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[J");

        return target;
    }

    public static TextWriter ClearToStartOfScreen(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[1J");

        return target;
    }

    public static TextWriter ClearScreen(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[2J");

        return target;
    }

    public static TextWriter ClearScreenAndBuffer(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);
        target.Write("\u001b[3J");

        return target;
    }

    public static TextWriter ScrollUp(this TextWriter target, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

        if (amount == 0)
        {
            return target;
        }

        target.Write($"\u001b[{amount}S");

        return target;
    }

    public static TextWriter ScrollDown(this TextWriter target, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentOutOfRangeException.ThrowIfLessThan(amount, 0);

        if (amount == 0)
        {
            return target;
        }

        target.Write($"\u001b[{amount}T");

        return target;
    }

    public static TextWriter SavePosition(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[s");

        return target;
    }

    public static TextWriter RestorePosition(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[u");

        return target;
    }

    public static TextWriter HideCursor(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[?25l");

        return target;
    }

    public static TextWriter ShowCursor(this TextWriter target)
    {
        ArgumentNullException.ThrowIfNull(target);

        target.Write("\u001b[?25h");

        return target;
    }
}