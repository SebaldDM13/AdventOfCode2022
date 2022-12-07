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
lines = File.ReadAllLines(Path.Combine(path, "Day05.txt"));
int index = lines.EmptyLineIndex();
List<List<char>> table1 = lines[..index].Pivot();
table1.RemoveAll(r => r[0] == ' ');
table1.ForEach(r => r.RemoveAll(c => c == ' '));
List<List<char>> table2 = table1.Copy();
foreach (string instruction in lines[(index + 1)..])
{
    string[] tokens = instruction.Split(' ');
    int quantity = int.Parse(tokens[1]);
    int source = int.Parse(tokens[3]) - 1;
    int destination = int.Parse(tokens[5]) - 1;
    table1[destination].AddRange(table1[source].Skip(table1[source].Count - quantity).Reverse());
    table2[destination].AddRange(table2[source].Skip(table2[source].Count - quantity));
    table1[source].RemoveRange(table1[source].Count - quantity, quantity);
    table2[source].RemoveRange(table2[source].Count - quantity, quantity);
}
Console.WriteLine("Part 1:");
Console.WriteLine(table1.Print());
Console.WriteLine();
Console.WriteLine("Part 2:");
Console.WriteLine(table2.Print());
Console.WriteLine();

Console.WriteLine("Day 06:");
string text = File.ReadAllText(Path.Combine(path, "Day06.txt"));
Console.WriteLine("Processed characters for marker length 4: " + (text.NonRepeatingChainIndex(4) + 4));
Console.WriteLine("Processed characters for marker length 14: " + (text.NonRepeatingChainIndex(14) + 14));
Console.WriteLine();

Console.WriteLine("Day 07:");
lines = File.ReadAllLines(Path.Combine(path, "Day07.txt"));
DirectoryNode current = new DirectoryNode("/");
List<DirectoryNode> allDirectories = new() { current };
bool isInListing = false;

foreach (string line in lines)
{
    string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (tokens[0] == "$")
    {
        isInListing = false;
        if (tokens[1] == "ls")
        {
            isInListing = true;
        }
        if (tokens[1] == "cd")
        {
            if (tokens[2] == "/")
            {
                current = allDirectories[0];
            }
            else if (tokens[2] == "..")
            {
                if (current.Parent is not null)
                {
                    current = current.Parent;
                }
            }
            else
            {
                current = current.SubDirectory(tokens[2]);
            }
        }
    }
    else if (isInListing)
    {
        if (tokens[0] == "dir")
        {
            DirectoryNode subDirectory = new DirectoryNode(tokens[1]);
            current.Add(subDirectory);
            allDirectories.Add(subDirectory);
        }
        else
        {
            current.Add(new FileNode(tokens[1], int.Parse(tokens[0])));
        }
    }
}

Console.WriteLine("Sum of directory sizes of at most 100000: " + allDirectories.Where(d => d.Size <= 100000).Sum(d => d.Size));
int used = allDirectories[0].Size;
int totalSpace = 70000000;
int unused = totalSpace - used;
int requiredSpace = 30000000;
int minimumSpaceToFreeUp = requiredSpace - unused;
Console.WriteLine("Used space: " + used);
Console.WriteLine("Unused space: " + unused);
Console.WriteLine("Minimum space to free up: " + minimumSpaceToFreeUp);
int smallestDirectorySizeOfMinimumSpace = allDirectories.Select(d => d.Size).Where(i => i >= minimumSpaceToFreeUp).Min();
Console.WriteLine("Smallest directory size of at least the minimum space needed: " + smallestDirectorySizeOfMinimumSpace);
Console.WriteLine("Directory name(s): " + string.Join(", ", allDirectories.Where(d => d.Size == smallestDirectorySizeOfMinimumSpace).Select(d => d.Name)));
