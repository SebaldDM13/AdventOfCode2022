namespace AdventOfCode2022;

public static class Extensions
{
    public static IEnumerable<int> Totals(this IEnumerable<string> lines)
    {
        int sum = 0;
        foreach (string s in lines)
        {
            if (s.Length == 0)
            {
                yield return sum;
                sum = 0;
            }
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
        return partitions.Select(p => p.CommonCharacters());
    }

    public static IEnumerable<T> Singles<T>(this IEnumerable<IEnumerable<T>> partitions)
    {
        return partitions.Select(p => p.Single());
    }

    public static IEnumerable<string> Halves(this string s)
    {
        yield return s[..(s.Length / 2)];
        yield return s[(s.Length / 2)..];
    }

    public static IEnumerable<IEnumerable<string>> Halves(this IEnumerable<string> lines)
    {
        return lines.Select(l => l.Halves());
    }

    public static int PriorityValue(this char c) => c switch
    {
        >= 'a' and <= 'z' => c - 'a' + 1,
        >= 'A' and <= 'Z' => c - 'A' + 27,
        _ => 0
    };

    public static IEnumerable<int> PriorityValues(this IEnumerable<char> chars)
    {
        return chars.Select(c => PriorityValue(c));
    }

    public static IEnumerable<string[]> Splits(this IEnumerable<string> strings, char separator)
    {
        return strings.Select(s => s.Split(separator));
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

    public static List<List<char>> Pivot(this string[] lines)
    {
        List<List<char>> table = new();
        for(int i = 0; i < lines[0].Length; i++)
        {
            List<char> row = new();
            for (int j = lines.Length - 1; j >= 0; j--)
            {
                row.Add(lines[j][i]);
            }
            table.Add(row);
        }
        return table;
    }

    public static List<List<char>> Copy(this List<List<char>> table)
    {
        List<List<char>> copy = new();
        foreach(List<char> row in table)
        {
            copy.Add(new(row));
        }
        return copy;
    }

    public static string Print(this IEnumerable<IEnumerable<char>> lines)
    {
        return string.Join(Environment.NewLine, lines.Select(s => new string(s.ToArray())));
    }
}
