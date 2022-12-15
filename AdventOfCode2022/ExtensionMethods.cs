using System.Collections.Specialized;
using System.Text;

namespace AdventOfCode2022;

public static class ExtensionMethods
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

    public static List<List<char>> TurnedClockwise(this string[] lines)
    {
        List<List<char>> table = new();
        for (int i = 0; i < lines[0].Length; i++)
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

    public static string ToText(this IEnumerable<IEnumerable<char>> lines)
    {
        return string.Join(Environment.NewLine, lines.Select(s => new string(s.ToArray())));
    }

    public static int[,] ToGrid(this string[] lines, Func<char, int> conversion)
    {
        int[,] grid = new int[lines.Length, lines[0].Length];
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                grid[y, x] = conversion(lines[y][x]);
            }
        }

        return grid;
    }

    public static (int, int) LineCharIndexOf(this string[] lines, char c)
    {
        for (int line = 0; line < lines.Length; line++)
        {
            int charIndex = lines[line].IndexOf(c);
            if (charIndex >= 0)
                return (line, charIndex);
        }

        return (-1, -1);
    }

    public static void Increment<T>(this Dictionary<T, int> dictionary, T key) where T : IEquatable<T>
    {
        dictionary.TryGetValue(key, out int value);
        dictionary[key] = value + 1;
    }

    public static void Decrement<T>(this Dictionary<T, int> dictionary, T key) where T : IEquatable<T>
    {
        dictionary.TryGetValue(key, out int value);
        dictionary[key] = value - 1;
    }

    public static long Product(this IEnumerable<int> ints)
    {
        long product = 1;
        foreach (int i in ints)
        {
            product *= i;
        }

        return product;
    }

    public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            queue.Enqueue(item);
        }
    }

    public static int CountWhile<T>(this IEnumerable<T> collection, Predicate<T> predicate)
    {
        IEnumerator<T> enumerator = collection.GetEnumerator();
        int count = 0;
        while (enumerator.MoveNext() && predicate(enumerator.Current))
        {
            count++;
        }

        return count;
    }
}
