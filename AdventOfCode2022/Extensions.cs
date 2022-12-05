using System.ComponentModel;

namespace AdventOfCode2022;

public static class Extensions
{
    public static IEnumerable<IEnumerable<string>> Partitions(this IEnumerable<string> lines)
    {
        IEnumerator<string> enumerator = lines.GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return NextPartition(enumerator);
        }
    }

    private static IEnumerable<string> NextPartition(IEnumerator<string> enumerator)
    {
        while (enumerator.Current != string.Empty)
        {
            yield return enumerator.Current;
            if (!enumerator.MoveNext())
                yield break;
        }
    }

    public static int Total(this IEnumerable<string> lines)
    {
        return lines.Sum(int.Parse);
    }

    public static IEnumerable<int> Totals(this IEnumerable<IEnumerable<string>> partitions)
    {
        foreach (var partition in partitions)
        {
            yield return partition.Total();
        }
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
}
