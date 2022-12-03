namespace AdventOfCode2022;

public static class GroupAdder
{
    public static IEnumerable<int> CalculateTotals(IEnumerable<string> lines, string delimiter)
    {
        int total = 0;
        foreach (string line in lines)
        {
            if (line == delimiter)
            {
                yield return total;
                total = 0;
            }
            else
            {
                total += int.Parse(line);
            }
        }

        yield return total;
    }

}
