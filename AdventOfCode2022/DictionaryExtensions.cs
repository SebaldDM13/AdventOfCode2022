namespace AdventOfCode2022;

static class DictionaryExtensions
{
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
}
