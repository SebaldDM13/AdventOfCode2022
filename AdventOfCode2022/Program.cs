using AdventOfCode2022;

const string path = "Input";
string[] lines;
Console.WriteLine("Advent of Code 2022");
Console.WriteLine();

Console.WriteLine("Day 01:");
lines = File.ReadAllLines(Path.Combine(path, "Day01.txt"));
Console.WriteLine("Most Calories total: " + lines.Totals().Max());
Console.WriteLine("Sum of top three Calorie totals: " + lines.Totals().OrderDescending().Take(3).Sum());
Console.WriteLine();

Console.WriteLine("Day 02:");
lines = File.ReadAllLines(Path.Combine(path, "Day02.txt"));
Console.WriteLine("Part 1 score (switch implementation): " + lines.Sum(RockPaperScissorsSwitchImplementation.ScoreOfOpponentMoveAndPlayerMove));
Console.WriteLine("Part 2 score (switch implementation): " + lines.Sum(RockPaperScissorsSwitchImplementation.ScoreOfOpponentMoveAndResult));
Console.WriteLine("Part 1 score (equation implementation): " + lines.Sum(RockPaperScissorsEquationImplementation.ScoreOfOpponentMoveAndPlayerMove));
Console.WriteLine("Part 2 score (equation implementation): " + lines.Sum(RockPaperScissorsEquationImplementation.ScoreOfOpponentMoveAndResult));
Console.WriteLine();

Console.WriteLine("Day 03:");
lines = File.ReadAllLines(Path.Combine(path, "Day03.txt"));
Console.WriteLine("Priority sum of common items: " + lines.Halves().CommonCharacters().Singles().PriorityValues().Sum());
Console.WriteLine("Priority sum of badges: " + lines.Chunk(3).CommonCharacters().Singles().PriorityValues().Sum());
Console.WriteLine();

Console.WriteLine("Day 04:");
lines = File.ReadAllLines(Path.Combine(path, "Day04.txt"));
IEnumerable<Range[]> ranges = lines.Splits(',').Select(s => new Range[] { new Range(s[0]), new Range(s[1]) });
Console.WriteLine("Count of pairs where a range fully contains the other: " + ranges.Count(r => r[0].FullyContains(r[1]) || r[1].FullyContains(r[0])));
Console.WriteLine("Count of overlapping range pairs: " + ranges.Count(r => r[0].Overlaps(r[1])));
Console.WriteLine();

Console.WriteLine("Day 05:");
Console.WriteLine();
lines = File.ReadAllLines(Path.Combine(path, "Day05.txt"));
RunCargoSimulation("Crate Mover 9000 results:", new CrateMover9000(), lines);
RunCargoSimulation("Crate Mover 9001 results:", new CrateMover9001(), lines);

static void RunCargoSimulation(string description, CrateMover mover, string[] lines)
{
    Console.WriteLine(description);

    int index = lines.EmptyLineIndex();
    IEnumerable<int> nonSpaceIndicies = lines[index - 1].NonSpaceIndicies();
    Stack<char>[] stacks = lines[..(index - 1)].Reverse().Columns(nonSpaceIndicies).ToStacks().ToArray();

    foreach (string instruction in lines[(index + 1)..])
    {
        mover.Move(stacks, instruction);
    }

    for (int i = 0; i < stacks.Length; i++)
    {
        Console.WriteLine($"{i + 1}: {new string(stacks[i].Reverse().ToArray())}");
    }
    Console.WriteLine("Top crates: " + new string(stacks.Select(s => s.Peek()).ToArray()));
    Console.WriteLine();
}
