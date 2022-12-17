using System.Reflection;

namespace AdventOfCode2022;

public class RangeList
{
    private readonly List<Range> range = new();

    public RangeList()
    {
    }

    public RangeList(Range item)
    {
        range.Add(item);
    }

    public void Clear()
    {
        range.Clear();
    }

    public int PieceCount => range.Count;

    public Range Piece(int i) => range[i];

    public int Count => range.Sum(r => r.Count);

    public bool Contains(int i) => range.Any(r => r.Contains(i));

    public override string ToString()
    {
        return string.Join(", ", range);
    }

    public void Cap(Range cap)
    {
        range.RemoveAll(r => r.End < cap.Start || r.Start > cap.End);
        if (range.Count > 0)
        {
            range[0] = new Range(Math.Max(range[0].Start, cap.Start), range[0].End);
            range[^1] = new Range(range[^1].Start, Math.Min(range[^1].End, cap.End));
        }
    }

    public void Add(Range item)
    {
        int i;
        for (i = 0; i < range.Count; i++)
        {
            if (range[i].FullyContains(item))
            {
                return;
            }
            else if (item.End < range[i].Start - 1)
            {
                break;
            }
            else if (item.Touches(range[i]))
            {
                item = new Range(Math.Min(item.Start, range[i].Start), Math.Max(item.End, range[i].End));
                range.RemoveAt(i);
                i--;
            }
        }

        range.Insert(i, item);
    }
}
