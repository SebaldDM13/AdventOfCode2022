namespace AdventOfCode2022;

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
        int dashIndex = s.IndexOf('-');
        if (dashIndex < 0)
        {
            Start = End = int.Parse(s);
        }
        else
        {
            Start = int.Parse(s[..dashIndex]);
            End = int.Parse(s[(dashIndex + 1)..]);
        }
    }

    public override string ToString()
    {
        return (Start == End) ? $"{Start}" : $"{Start}-{End}";
    }

    public bool FullyContains(Range other)
    {
        return Start <= other.Start && other.End <= End;
    }

    public bool Overlaps(Range other)
    {
        return other.Start <= End && Start <= other.End;
    }

    public bool Touches(Range other)
    {
        return other.Start <= End + 1 && Start <= other.End + 1;
    }

    public bool Contains(int i)
    {
        return Start <= i && i <= End;
    }

    public int Count => End - Start + 1;

    public void Deconstruct(out int start, out int end)
    {
        start = Start;
        end = End;
    }
}
