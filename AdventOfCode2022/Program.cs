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
Dictionary<char, int> charToRockPaperScissors123 = new() { { 'A', 1 }, { 'B', 2 }, { 'C', 3 }, { 'X', 1 }, { 'Y', 2 }, { 'Z', 3 } };
Dictionary<char, int> charToLoseTieWin012 = new() { { 'X', 0 }, { 'Y', 1 }, { 'Z', 2 } };
Func<int, int, int> result = (yourMove, opponentMove) => (yourMove - opponentMove + 4) % 3;
Func<int, int, int> yourMove = (opponentMove, result) => (opponentMove + result + 1) % 3 + 1;
int score1 = 0;
int score2 = 0;
foreach (string line in lines)
{
    int opponentMove = charToRockPaperScissors123[line[0]];
    int yourPart1Move = charToRockPaperScissors123[line[2]];
    int part2Result = charToLoseTieWin012[line[2]];
    score1 += yourPart1Move + 3 * result(yourPart1Move, opponentMove);
    score2 += yourMove(opponentMove, part2Result) + 3 * part2Result;
}

Console.WriteLine("Part 1 score: " + score1);
Console.WriteLine("Part 2 score: " + score2);
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
FileSystem fileSystem = new();
fileSystem.Initialize(lines);
Console.WriteLine("Sum of directory sizes of at most 100000: " + fileSystem.AllDirectories.Where(d => d.Size <= 100000).Sum(d => d.Size));
int used = fileSystem.AllDirectories[0].Size;
int totalSpace = 70000000;
int unused = totalSpace - used;
int requiredSpace = 30000000;
int minimumSpaceToFreeUp = requiredSpace - unused;
int smallestDirectorySizeOfMinimumSpace = fileSystem.AllDirectories.Select(d => d.Size).Where(i => i >= minimumSpaceToFreeUp).Min();
Console.WriteLine("Used space: " + used);
Console.WriteLine("Unused space: " + unused);
Console.WriteLine("Minimum space to free up: " + minimumSpaceToFreeUp);
Console.WriteLine("Smallest directory size of at least the minimum space needed: " + smallestDirectorySizeOfMinimumSpace);
Console.WriteLine("Directory name(s): " + string.Join(", ", fileSystem.AllDirectories.Where(d => d.Size == smallestDirectorySizeOfMinimumSpace).Select(d => d.Name)));
Console.WriteLine();

Console.WriteLine("Day 08:");
lines = File.ReadAllLines(Path.Combine(path, "Day08.txt"));
int[,] heightMap = lines.ToGrid();
int visibleTreeCount = 0;
int maxScenicScore = 0;
Vector2Int[] directions = new[] { Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right };
for (Vector2Int location = new(); location.Y < heightMap.GetLength(1); location.Y++)
{
    for (location.X = 0; location.X < heightMap.GetLength(0); location.X++)
    {
        bool visibility = false;
        int scenicScore = 1;
        foreach (Vector2Int direction in directions)
        {
            bool directionVisibility = true;
            int directionScenicScore = 0;
            for (Vector2Int scan = location + direction; scan.IsWithin(heightMap); scan += direction)
            {
                directionScenicScore++;
                if (heightMap[scan.Y, scan.X] >= heightMap[location.Y, location.X])
                {
                    directionVisibility = false;
                    break;
                }
            }
            visibility |= directionVisibility;
            scenicScore *= directionScenicScore;
        }

        visibleTreeCount += visibility ? 1 : 0;
        maxScenicScore = Math.Max(maxScenicScore, scenicScore);
    }
}

Console.WriteLine("Visible tree count: " + visibleTreeCount);
Console.WriteLine("Highest possible scenic score: " + maxScenicScore);
Console.WriteLine();

Console.WriteLine("Day 09:");
lines = File.ReadAllLines(Path.Combine(path, "Day09.txt"));
Dictionary<string, Vector2Int> stringToDirection = new() { { "U", Vector2Int.Up }, { "D", Vector2Int.Down }, { "L", Vector2Int.Left }, { "R", Vector2Int.Right } };
Vector2Int[] rope = new Vector2Int[10];
HashSet<Vector2Int>[] knotHistories = rope.Select(knot => new HashSet<Vector2Int>() { knot }).ToArray();
foreach (string line in lines)
{
    string[] tokens = line.Split(' ');
    int moves = int.Parse(tokens[1]);
    for (int move = 0; move < moves; move++)
    {
        rope[0] += stringToDirection[tokens[0]];
        knotHistories[0].Add(rope[0]);
        for (int knot = 1; knot < rope.Length; knot++)
        {
            Vector2Int difference = rope[knot - 1] - rope[knot];
            if (difference.Abs().Max() == 2)
            {
                rope[knot] += difference.Sign();
            }

            knotHistories[knot].Add(rope[knot]);
        }
    }
}

Console.WriteLine("Positions length 1 rope tail visited: " + knotHistories[1].Count);
Console.WriteLine("Positions length 9 rope tail visited: " + knotHistories[9].Count);
Console.WriteLine();
