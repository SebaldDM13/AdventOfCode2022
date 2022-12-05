public readonly struct Range
{
    public readonly int Start { get; init; }
    public readonly int End { get; init; }

    public Range(int start, int end)
    {
        Start = start;
        End = end;
    }

    public Range(string s)
    {
        string[] strings = s.Split('-');
        Start = int.Parse(strings[0].Trim());
        End = int.Parse(strings[1].Trim());
    }

    public override string ToString()
    {
        return $"{Start}-{End}";
    }

    public bool FullyContains(Range other)
    {
        return Start <= other.Start && other.End <= End;
    }

    public bool Overlaps(Range other)
    {
        return other.Start <= End && Start <= other.End;
    }
}
