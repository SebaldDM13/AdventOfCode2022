namespace AdventOfCode2022;

public static class RangeExtensions
{
    public static IEnumerable<Range> Capped(this IEnumerable<Range> ranges, Range cap)
    {
        foreach (Range range in ranges)
        {
            if (range.Overlaps(cap))
            {
                yield return new Range(Math.Max(range.Start, cap.Start), Math.Min(range.End, cap.End));
            }
        }
    }

    public static IEnumerable<Range> Union(this IEnumerable<Range> ranges)
    {
        bool isFirst = true;
        int joinStart = 0;
        int joinEnd = 0;
        foreach (Range range in ranges.OrderBy(r => r.Start))
        {
            if (isFirst)
            {
                joinStart = range.Start;
                joinEnd = range.End;
                isFirst = false;
            }
            else if (range.Start > joinEnd + 1)
            {
                yield return new Range(joinStart, joinEnd);
                joinStart = range.Start;
                joinEnd = range.End;
            }
            else if (range.End > joinEnd)
            {
                joinEnd = range.End;
            }
        }

        if (!isFirst)
        {
            yield return new Range(joinStart, joinEnd);
        }
    }
}
