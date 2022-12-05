namespace AdventOfCode2022;

public static class Extensions
{
    public static IEnumerable<int> Totals(this IEnumerable<string> lines)
    {
        int sum = 0;
        foreach (string s in lines)
        {
            if(s.Length == 0)
                yield return sum;
            else
                sum += int.Parse(s);
        }
        yield return sum;
    }

    public static IEnumerable<char> CommonCharacters(this IEnumerable<string> strings)
    {
        IEnumerator<string> enumerator = strings.GetEnumerator();
        enumerator.MoveNext();
        IEnumerable<char> common = enumerator.Current;
        while (enumerator.MoveNext())
        {
            common = common.Intersect(enumerator.Current);
        }

        return common;
    }

    public static IEnumerable<IEnumerable<char>> CommonCharacters(this IEnumerable<IEnumerable<string>> partitions)
    {
        foreach (IEnumerable<string> strings in partitions)
        {
            yield return strings.CommonCharacters();
        }
    }

    public static IEnumerable<T> Singles<T>(this IEnumerable<IEnumerable<T>> partitions)
    {
        foreach (IEnumerable<T> p in partitions)
        {
            yield return p.Single();
        }
    }

    public static IEnumerable<string> Halves(this string s)
    {
        yield return s[..(s.Length / 2)];
        yield return s[(s.Length / 2)..];
    }

    public static IEnumerable<IEnumerable<string>> Halves(this IEnumerable<string> lines)
    {
        foreach (string line in lines)
        {
            yield return line.Halves();
        }
    }

    public static int PriorityValue(this char c) => c switch
    {
        >= 'a' and <= 'z' => c - 'a' + 1,
        >= 'A' and <= 'Z' => c - 'A' + 27,
        _ => 0
    };

    public static IEnumerable<int> PriorityValues(this IEnumerable<char> chars)
    {
        foreach (char c in chars)
        {
            yield return c.PriorityValue();
        }
    }

    public static IEnumerable<string[]> Splits(this IEnumerable<string> strings, char separator)
    {
        foreach (string s in strings)
        {
            yield return s.Split(separator);
        }
    }

    public static int EmptyLineIndex(this string[] lines)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length == 0)
                return i;
        }

        return -1;
    }

    public static IEnumerable<int> NonSpaceIndicies(this string s)
    {
        for(int i = 0; i < s.Length; i++)
        {
            if (s[i] != ' ')
                yield return i;
        }
    }

    public static IEnumerable<IEnumerable<char>> Columns(this IEnumerable<string> lines, IEnumerable<int> indicies)
    {
        return indicies.Select(i => lines.Column(i));
    }

    public static IEnumerable<char> Column(this IEnumerable<string> lines, int index)
    {
        return lines.Select(s => index < s.Length ? s[index] : ' ');
    }

    public static IEnumerable<Stack<char>> ToStacks(this IEnumerable<IEnumerable<char>> chars)
    {
        return chars.Select(ToStack);
    }

    public static Stack<char> ToStack(this IEnumerable<char> chars)
    {
        return new Stack<char>(chars.TakeWhile(c => c != ' '));
    }
}
