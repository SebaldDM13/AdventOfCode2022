using AdventOfCode2022;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Range = AdventOfCode2022.Range;

Console.WriteLine("Advent of Code 2022");
Console.WriteLine();
Stopwatch stopwatch = new();

Action<string[]>[] days = new Action<string[]>[] { Day01, Day02, Day03, Day04, Day05, Day06, Day07, Day08, Day09, Day10, Day11, Day12, Day13, Day14, Day15, Day16, Day17, Day18, Day19, Day20, Day21, Day22, Day23, Day24, Day25 };
for(int day = 0; day < days.Length; day++)
{
    stopwatch.Restart();
    Console.WriteLine("Day " + (day + 1).ToString("00") + ":");
    string[] lines = File.ReadAllLines(Path.Combine("Input", "Day" + (day + 1).ToString("00") + ".txt"));
    days[day](lines);
    stopwatch.Stop();
    Console.WriteLine(stopwatch.ElapsedMilliseconds + " ms");
    Console.WriteLine();
}

static void Day01(string[] lines)
{
    Console.WriteLine("Most Calories total: " + lines.Totals().Max());
    Console.WriteLine("Sum of top three Calorie totals: " + lines.Totals().OrderDescending().Take(3).Sum());
}

static void Day02(string[] lines)
{
    static int to_1_rock_2_paper_3_scissors(char c) => c switch { 'A' => 1, 'B' => 2, 'C' => 3, 'X' => 1, 'Y' => 2, 'Z' => 3, _ => 1 };
    static int to_0_lose_1_tie_2_win(char c) => c switch { 'X' => 0, 'Y' => 1, 'Z' => 2, _ => 0 };
    static int rockPaperScissorsResult(int yourMove, int opponentMove) => (yourMove - opponentMove + 4) % 3;
    static int yourMove(int opponentMove, int result) => (opponentMove + result + 1) % 3 + 1;
    int score1 = 0;
    int score2 = 0;
    foreach (string line in lines)
    {
        int opponentMove = to_1_rock_2_paper_3_scissors(line[0]);
        int yourPart1Move = to_1_rock_2_paper_3_scissors(line[2]);
        int part2Result = to_0_lose_1_tie_2_win(line[2]);
        score1 += yourPart1Move + 3 * rockPaperScissorsResult(yourPart1Move, opponentMove);
        score2 += yourMove(opponentMove, part2Result) + 3 * part2Result;
    }

    Console.WriteLine("Part 1 score: " + score1);
    Console.WriteLine("Part 2 score: " + score2);
}

static void Day03(string[] lines)
{
    static int priorityValue(char c) => c switch { >= 'a' and <= 'z' => c - 'a' + 1, >= 'A' and <= 'Z' => c - 'A' + 27, _ => 0 };
    Console.WriteLine("Priority sum of common items: " + lines.Halves().CommonCharacters().Singles().Sum(priorityValue));
    Console.WriteLine("Priority sum of badges: " + lines.Chunk(3).CommonCharacters().Singles().Sum(priorityValue));
}

static void Day04(string[] lines)
{
    IEnumerable<Range[]> ranges = lines.Splits(',').Select(s => new []{ new Range(s[0]), new Range(s[1]) });
    Console.WriteLine("Count of pairs where a range fully contains the other: " + ranges.Count(r => r[0].FullyContains(r[1]) || r[1].FullyContains(r[0])));
    Console.WriteLine("Count of overlapping range pairs: " + ranges.Count(r => r[0].Overlaps(r[1])));
}

static void Day05(string[] lines)
{
    int index = lines.EmptyLineIndex();
    List<List<char>> table1 = lines[..index].TurnedClockwise();
    table1.RemoveAll(r => r[0] == ' ');
    table1.ForEach(r => r.RemoveAll(c => c == ' '));
    List<List<char>> table2 = new(table1.Select(r => new List<char>(r)));
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
    Console.WriteLine();
    Console.WriteLine("Part 1:");
    Console.WriteLine(table1.ToText());
    Console.WriteLine();
    Console.WriteLine("Part 2:");
    Console.WriteLine(table2.ToText());
    Console.WriteLine();
}

static void Day06(string[] lines)
{
    static int processedCharactersToDetectMarker(string buffer, int markerLength)
    {
        Dictionary<char, int> characterCounts = new();
        for (int i = 0; i < buffer.Length; i++)
        {
            if (i >= markerLength)
            {
                if (characterCounts.Values.All(v => v <= 1))
                    return i;

                characterCounts.Decrement(buffer[i - markerLength]);
            }

            characterCounts.Increment(buffer[i]);
        }

        return -1;
    }

    Console.WriteLine("Processed characters for marker length 4: " + processedCharactersToDetectMarker(lines[0], 4));
    Console.WriteLine("Processed characters for marker length 14: " + processedCharactersToDetectMarker(lines[0], 14));
}

static void Day07(string[] lines)
{
    FileSystem fileSystem = new(lines);
    Console.WriteLine("Sum of directory sizes of at most 100000: " + fileSystem.DirectoryTraversal().Where(d => d.Size <= 100000).Sum(d => d.Size));
    int used = fileSystem.Root.Size;
    int totalSpace = 70000000;
    int unused = totalSpace - used;
    int requiredSpace = 30000000;
    int minimumSpaceToFreeUp = requiredSpace - unused;
    int smallestDirectorySizeOfMinimumSpace = fileSystem.DirectoryTraversal().Select(d => d.Size).Where(i => i >= minimumSpaceToFreeUp).Min();
    Console.WriteLine("Used space: " + used);
    Console.WriteLine("Unused space: " + unused);
    Console.WriteLine("Minimum space to free up: " + minimumSpaceToFreeUp);
    Console.WriteLine("Smallest directory size of at least the minimum space needed: " + smallestDirectorySizeOfMinimumSpace);
    Console.WriteLine("Directory name(s): " + string.Join(", ", fileSystem.DirectoryTraversal().Where(d => d.Size == smallestDirectorySizeOfMinimumSpace).Select(d => d.Name)));
}

static void Day08(string[] lines)
{
    int[,] heightMap = lines.ToGrid(c => c - '0');
    int visibleTreeCount = 0;
    int maxScenicScore = 0;
    for (Vector2Int location = new(); location.Y < heightMap.GetLength(1); location.Y++)
    {
        for (location.X = 0; location.X < heightMap.GetLength(0); location.X++)
        {
            bool visibility = false;
            int scenicScore = 1;
            foreach (Vector2Int direction in Vector2Int.Directions)
            {
                bool directionVisibility = true;
                int directionScenicScore = 0;
                for (Vector2Int scan = location + direction; scan.InBounds(heightMap); scan += direction)
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
}

static void Day09(string[] lines)
{
    Vector2Int[] rope = new Vector2Int[10];
    HashSet<Vector2Int>[] knotHistories = rope.Select(knot => new HashSet<Vector2Int>() { knot }).ToArray();
    foreach (string line in lines)
    {
        string[] tokens = line.Split(' ');
        Vector2Int direction = tokens[0][0] switch { 'U' => Vector2Int.Up, 'D' => Vector2Int.Down, 'L' => Vector2Int.Left, 'R' => Vector2Int.Right, _ => Vector2Int.Zero };
        int moves = int.Parse(tokens[1]);
        for (int move = 0; move < moves; move++)
        {
            rope[0] += direction;
            knotHistories[0].Add(rope[0]);
            for (int knot = 1; knot < rope.Length; knot++)
            {
                Vector2Int difference = rope[knot - 1] - rope[knot];
                if (difference.Abs().Max() >= 2)
                {
                    rope[knot] += difference.Sign();
                }

                knotHistories[knot].Add(rope[knot]);
            }
        }
    }

    Console.WriteLine("Positions length 1 rope tail visited: " + knotHistories[1].Count);
    Console.WriteLine("Positions length 9 rope tail visited: " + knotHistories[9].Count);
}

static void Day10(string[] lines)
{
    string[] tokens = lines.SelectMany(line => line.Split(' ')).ToArray();
    HashSet<int> cyclesOfInterest = new() { 20, 60, 100, 140, 180, 220 };
    int registerX = 1;
    int signalStrengthSum = 0;
    char[] pixels = new char[240];
    
    for (int cycle = 1; cycle <= pixels.Length; cycle++)
    {
        if (cyclesOfInterest.Contains(cycle))
            signalStrengthSum += registerX * (cycle);

        pixels[cycle - 1] = (Math.Abs((cycle - 1) % 40 - registerX) <= 1) ? '#' : '.';

        if (cycle >= 2 && tokens[cycle - 2] == "addx")
            registerX += int.Parse(tokens[cycle - 1]);
    }

    Console.WriteLine("Signal strength sum: " + signalStrengthSum);
    Console.WriteLine();
    pixels.Chunk(40).ToList().ForEach(row => Console.WriteLine(row));
}

static void Day11(string[] lines)
{
    static long MonkeyBusinessLevel() => Monkey.Troop.Select(m => m.TimesInspectedItems).OrderDescending().Take(2).Product();

    Monkey.Troop.AddRange(lines.Chunk(7).Select(chunk => new Monkey(chunk)));
    for (int t = 0; t < 20; t++)
        Monkey.Troop.ForEach(m => m.TakeTurn(w => w / 3));
    Console.WriteLine("Level of monkey business (Part 1): " + MonkeyBusinessLevel());

    long product = Monkey.Troop.Select(m => m.ModuloDivisor).Product();
    Monkey.Troop.ForEach(m => m.Reset());
    for (int t = 0; t < 10000; t++)
        Monkey.Troop.ForEach(m => m.TakeTurn(w => w % product));
    Console.WriteLine("Level of monkey business (Part 2): " + MonkeyBusinessLevel());
}

static void Day12(string[] lines)
{
    int minStepCount(int[,] heightMap, Vector2Int start, Vector2Int end)
    {
        HashSet<Vector2Int> open = new() { start };
        HashSet<Vector2Int> closed = new();
        Dictionary<Vector2Int, int> gCost = new() { { start, 0 } };
        int hCost(Vector2Int v) => Vector2Int.ManhattanDistance(v, end);
        int fCost(Vector2Int v) => gCost[v] + hCost(v);

        while (true)
        {
            Vector2Int current = open.MinBy(v => fCost(v));
            open.Remove(current);
            closed.Add(current);

            if (current == end)
                return gCost[current];

            int currentHeight = heightMap[current.Y, current.X];
            foreach (Vector2Int direction in Vector2Int.Directions)
            {
                Vector2Int neighbor = current + direction;
                if (neighbor.InBounds(heightMap) && heightMap[neighbor.Y, neighbor.X] <= currentHeight + 1 && !closed.Contains(neighbor))
                {
                    bool neighborHasExistingPath = gCost.TryGetValue(neighbor, out int neighborPath);
                    if (!open.Contains(neighbor) || (neighborHasExistingPath && gCost[current] + 1 < neighborPath))
                    {
                        gCost[neighbor] = gCost[current] + 1;
                        open.Add(neighbor);
                    }
                }
            }
        }
    }

    int minStepCountFromHeight(int[,] heightMap, int startHeight, Vector2Int end)
    {
        HashSet<Vector2Int> open = new() { end };
        HashSet<Vector2Int> closed = new();
        Dictionary<Vector2Int, int> gCost = new() { { end, 0 } };

        while (true)
        {
            Vector2Int current = open.MinBy(v => gCost[v]);
            open.Remove(current);
            closed.Add(current);

            int currentHeight = heightMap[current.Y, current.X];
            if (currentHeight == 1)
                return gCost[current];

            foreach (Vector2Int direction in Vector2Int.Directions)
            {
                Vector2Int neighbor = current + direction;
                if (neighbor.InBounds(heightMap) && heightMap[neighbor.Y, neighbor.X] >= currentHeight - 1 && !closed.Contains(neighbor))
                {
                    bool neighborHasExistingPath = gCost.TryGetValue(neighbor, out int neighborPath);
                    if (!open.Contains(neighbor) || (neighborHasExistingPath && gCost[current] + 1 < neighborPath))
                    {
                        gCost[neighbor] = gCost[current] + 1;
                        open.Add(neighbor);
                    }
                }
            }
        }
    }

    int[,] heightMap = lines.ToGrid(c => c switch { 'S' => 1, 'E' => 26, _ => c - 'a' + 1 });
    (int startLine, int startChar) = lines.LineCharIndexOf('S');
    (int endLine, int endChar) = lines.LineCharIndexOf('E');
    Vector2Int start = new(startChar, startLine);
    Vector2Int end = new(endChar, endLine);
    Console.WriteLine("Step count (Part 1): " + minStepCount(heightMap, start, end));
    Console.WriteLine("Step count (Part 2): " + minStepCountFromHeight(heightMap, 1, end));
}

static void Day13(string[] lines)
{
    static bool IsInCorrectOrder(string a, string b)
    {
        List<string> aTokens = BracketOrIntegerRegex().Matches(a).Select(m => m.Value).ToList();
        List<string> bTokens = BracketOrIntegerRegex().Matches(b).Select(m => m.Value).ToList();
        for(int i = 0; i < aTokens.Count && i < bTokens.Count; i++)
        {
            bool aIsValue = int.TryParse(aTokens[i], out int aValue);
            bool bIsValue = int.TryParse(bTokens[i], out int bValue);

            if (aTokens[i] == "]" && bTokens[i] != "]")
            {
                return true;
            }
            else if (aTokens[i] != "]" && bTokens[i] == "]")
            {
                return false;
            }
            else if (aTokens[i] == "[" && bIsValue)
            {
                bTokens.Insert(i + 1, "]");
                bTokens.Insert(i, "[");
            }
            else if (aIsValue && bTokens[i] == "[")
            {
                aTokens.Insert(i + 1, "]");
                aTokens.Insert(i, "[");
            }
            else if (aIsValue && bIsValue)
            {
                if (aValue < bValue)
                    return true;
                else if (aValue > bValue)
                    return false;
            }
        }

        return aTokens.Count <= bTokens.Count;
    }

    List<int> indicesOfPacketsInCorrectOrder = new();
    for (int i = 0; i < lines.Length; i += 3)
    {
        if (IsInCorrectOrder(lines[i], lines[i + 1]))
            indicesOfPacketsInCorrectOrder.Add(1 + i / 3);
    }

    Console.WriteLine("Sum of indices of packets in correct order: " + indicesOfPacketsInCorrectOrder.Sum());
    int indexOfDividerPacket1 = lines.Count(s => s.Length > 0 && IsInCorrectOrder(s, "[[2]]")) + 1;
    int indexOfDividerPacket2 = lines.Count(s => s.Length > 0 && IsInCorrectOrder(s, "[[6]]")) + 2;
    Console.WriteLine("Index of divider packet 1: " + indexOfDividerPacket1);
    Console.WriteLine("Index of divider packet 2: " + indexOfDividerPacket2);
    Console.WriteLine("Product of divider packet indices: " + indexOfDividerPacket1 * indexOfDividerPacket2);
}

static void Day14(string[] lines)
{
    HashSet<Vector2Int> rock = new();
    foreach (string line in lines)
    {
        Vector2Int[] waypoints = line.Split(" -> ").Select(s => new Vector2Int(s)).ToArray();
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Vector2Int direction = (waypoints[i + 1] - waypoints[i]).Sign();
            for (Vector2Int point = waypoints[i]; point != waypoints[i + 1]; point += direction)
            {
                rock.Add(point);
            }
        }

        rock.Add(waypoints[^1]);
    }

    int bottom = rock.Select(v => v.Y).Max();
    HashSet<Vector2Int> occupied = new(rock);
    Vector2Int inflow = new(500, 0);
    Vector2Int sandPosition = inflow;
    while (sandPosition.Y < bottom)
    {
        sandPosition.Y++;
        if (occupied.Contains(sandPosition))
        {
            sandPosition.X--;
            if (occupied.Contains(sandPosition))
            {
                sandPosition.X += 2;
                if (occupied.Contains(sandPosition))
                {
                    sandPosition.X--;
                    sandPosition.Y--;
                    occupied.Add(sandPosition);
                    sandPosition = inflow;
                }
            }
        }
    }

    Console.WriteLine("Units of sand that came to a rest (Part 1): " + (occupied.Count - rock.Count));

    occupied.Clear();
    occupied.UnionWith(rock);
    bottom += 2;
    sandPosition = inflow;
    while (true)
    {
        sandPosition.Y++;
        if (sandPosition.Y >= bottom || occupied.Contains(sandPosition))
        {
            sandPosition.X--;
            if (sandPosition.Y >= bottom || occupied.Contains(sandPosition))
            {
                sandPosition.X += 2;
                if (sandPosition.Y >= bottom || occupied.Contains(sandPosition))
                {
                    sandPosition.X--;
                    sandPosition.Y--;
                    occupied.Add(sandPosition);

                    if (sandPosition == inflow)
                        break;

                    sandPosition = inflow;
                }
            }
        }
    }

    Console.WriteLine("Units of sand that came to a rest (Part 2): " + (occupied.Count - rock.Count));
}

static void Day15(string[] lines)
{
    Vector2Int[] sensorLocations = new Vector2Int[lines.Length];
    Vector2Int[] beaconLocations = new Vector2Int[lines.Length];
    int[] distances = new int[lines.Length];
    for (int line = 0; line < lines.Length; line++)
    {
        MatchCollection matches = IntegerRegex().Matches(lines[line]);
        sensorLocations[line] = new Vector2Int(int.Parse(matches[0].Value), int.Parse(matches[1].Value));
        beaconLocations[line] = new Vector2Int(int.Parse(matches[2].Value), int.Parse(matches[3].Value));
        distances[line] = Vector2Int.ManhattanDistance(sensorLocations[line], beaconLocations[line]);
    }

    RangeList rangeListForRow(int row)
    {
        RangeList sensedRangeList = new();
        for (int i = 0; i < sensorLocations.Length; i++)
        {
            int rowDistance = Math.Abs(row - sensorLocations[i].Y);
            int possibleColumnDistance = distances[i] - rowDistance;
            if (possibleColumnDistance >= 0)
            {
                sensedRangeList.Add(new Range(sensorLocations[i].X - possibleColumnDistance, sensorLocations[i].X + possibleColumnDistance));
            }
        }
        return sensedRangeList;
    }
        
    bool useSmallSearchSpace = sensorLocations.All(s => s.X <= 20 && s.Y <= 20);
    int targetRow = useSmallSearchSpace ? 10 : 2000000;
    RangeList sensedRangeList = rangeListForRow(targetRow);
    int positionCount = sensedRangeList.Count - beaconLocations.Distinct().Count(b => b.Y == targetRow && sensedRangeList.Contains(b.X));
    Console.WriteLine($"Count of positions in row {targetRow} known not to have a beacon: {positionCount}");
    for (int row = useSmallSearchSpace ? 20 : 4000000; row >= 0; row--)
    {
        sensedRangeList = rangeListForRow(row);
        if (sensedRangeList.PieceCount > 1)
        {
            int column = sensedRangeList.Piece(0).End + 1;
            long tuningFrequency = column * 4000000L + row;
            Console.WriteLine($"Distress beacon is at x={column}, y={row}");
            Console.WriteLine("Tuning frequency: " + tuningFrequency);
            break;
        }
    }
}

static void Day16(string[] lines)
{
}

static void Day17(string[] lines)
{
}

static void Day18(string[] lines)
{
}

static void Day19(string[] lines)
{
}

static void Day20(string[] lines)
{
}

static void Day21(string[] lines)
{
}

static void Day22(string[] lines)
{
}

static void Day23(string[] lines)
{
}

static void Day24(string[] lines)
{
}

static void Day25(string[] lines)
{
}

partial class Program
{
    [GeneratedRegex(@"-?\d+")]
    private static partial Regex IntegerRegex();

    [GeneratedRegex(@"\[|\]|-?\d+")]
    private static partial Regex BracketOrIntegerRegex();
}