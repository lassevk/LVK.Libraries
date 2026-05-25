using System.Globalization;

namespace LVK.Hosting.ConsoleApplications;

public static class ProgressBar
{
    private static readonly char[] _blocks = [
        ' ',
        '\u258f',
        '\u258e',
        '\u258d',
        '\u258c',
        '\u258b',
        '\u258a',
        '\u2589',
        '\u2588',
    ];

    public static readonly int LengthNeeded = 34;

    public static bool TryFormat(Span<char> target, int current, int total, out int charsWritten)
    {
        if (total < 0)
            throw new ArgumentOutOfRangeException(nameof(total), "total must be zero or greater");

        if (current < 0 || current > total)
            throw new ArgumentOutOfRangeException(nameof(current), "current must be 0 or less-than-or-equal-to total");

        if (target.Length < LengthNeeded)
        {
            charsWritten = 0;
            return false;
        }

        decimal percent = current * 100.0M / total;
        decimal blockCount = percent / 4M;

        var whole = (int)Math.Floor(blockCount);
        var fraction = (int)Math.Floor((blockCount - Math.Floor(blockCount)) * 8);

        // result = "[                         ] 100.0%"
        //                     1         2         3
        //           0123456789012345678901234567890123
        target[0] = '[';
        target[26] = ']';
        target[27] = ' ';
        target[33] = '%';
        int padding = 0;
        if (percent < 10.0M)
            padding = 2;
        else if (percent < 100.0M)
            padding = 1;

        target[28] = ' ';
        target[29] = ' ';
        percent.TryFormat(target[(28 + padding)..], out int _, "0.0", CultureInfo.InvariantCulture);

        for (var index = 0; index < whole; index++)
            target[index + 1] = '\u2588';
        if (fraction != 0)
        {
            target[whole + 1] = _blocks[fraction];
            for (int index = whole + 1; index < 25; index++)
                target[index + 1] = ' ';
        }
        else
        {
            for (int index = whole; index < 25; index++)
                target[index + 1] = ' ';
        }

        charsWritten = 34;
        return true;
    }

    public static string Format(int current, int total)
    {
        Span<char> buffer = stackalloc char[LengthNeeded];
        if (!TryFormat(buffer, current, total, out _))
            throw new InvalidOperationException("Internal error, unable to format progress bar");

        return buffer.ToString();
    }
}