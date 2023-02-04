namespace AdventOfCode2022;

static class DictionaryExtensions
{
    public static void Increment<T>(this Dictionary<T, int> dictionary, T key) where T : IEquatable<T>
    {
        dictionary.TryGetValue(key, out int value);
        dictionary[key] = value + 1;
    }

    public static void Increase<T>(this Dictionary<T, int> dictionary, T key, int amount) where T : IEquatable<T>
    {
        dictionary.TryGetValue(key, out int value);
        dictionary[key] = value + amount;
    }

    public static void Increase<T>(this Dictionary<T, int> dictionary, Dictionary<T, int> addend) where T : IEquatable<T>
    {
        foreach (KeyValuePair<T, int> pair in addend)
        {
            dictionary.Increase(pair.Key, pair.Value);
        }
    }

    public static void Decrement<T>(this Dictionary<T, int> dictionary, T key) where T : IEquatable<T>
    {
        dictionary.TryGetValue(key, out int value);
        dictionary[key] = value - 1;
    }

    public static void Decrease<T>(this Dictionary<T, int> dictionary, T key, int amount) where T : IEquatable<T>
    {
        dictionary.TryGetValue(key, out int value);
        dictionary[key] = value - amount;
    }

    public static void Decrease<T>(this Dictionary<T, int> dictionary, Dictionary<T, int> subtrahend) where T : IEquatable<T>
    {
        foreach (KeyValuePair<T, int> pair in subtrahend)
        {
            dictionary.Decrease(pair.Key, pair.Value);
        }
    }
        
    public static bool Satisfies<T>(this Dictionary<T, int> dictionary, Dictionary<T, int> requirements) where T : IEquatable<T>
    {
        foreach (KeyValuePair<T, int> requirement in requirements)
        {
            dictionary.TryGetValue(requirement.Key, out int value);
            if (value < requirement.Value)
            {
                return false;
            }
        }

        return true;
    }
}
