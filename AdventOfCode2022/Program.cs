using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2022;

public static partial class Program
{
    [GeneratedRegex(@"-?\d+")]
    private static partial Regex IntegerRegex();

    [GeneratedRegex(@"\[|\]|-?\d+")]
    private static partial Regex BracketOrIntegerRegex();

    [GeneratedRegex(@"[A-Z]{2}")]
    private static partial Regex TwoCapitalsRegex();

    [GeneratedRegex(@"\d+|[RL]")]
    private static partial Regex WholeNumberOrRLRegex();

    static void Main()
    {
        Console.WriteLine("Advent of Code 2022");
        Console.WriteLine();
        Stopwatch stopwatch = new();

        Action<string[]>[] days = new Action<string[]>[] { Day01, Day02, Day03, Day04, Day05, Day06, Day07, Day08, Day09, Day10, Day11, Day12, Day13, Day14, Day15, Day16, Day17, Day18, Day19, Day20, Day21, Day22, Day23, Day24, Day25 };
        string[][] yourPuzzleAnswers = new[] {
            new[] { "70509", "208567" },
            new[] { "12586", "13193" },
            new[] { "7763", "2659" },
            new[] { "500", "815" },
            new[] { "MQSHJMWNH", "LLWJRBHVZ" },
            new[] { "1175", "3217" },
            new[] { "1297683", "5756764" },
            new[] { "1789", "314820" },
            new[] { "6311", "2482" },
            new[] { "13220", "RUAKHBEK" },
            new[] { "120056", "21816744824" },
            new[] { "420", "414" },
            new[] { "4643", "21614" },
            new[] { "805", "25161" },
            new[] { "6275922", "11747175442119" },
            new[] { "?", "?" },
            new[] { "?", "?" },
            new[] { "4308", "2540" },
            new[] { "?", "?" },
            new[] { "?", "?" },
            new[] { "121868120894282", "3582317956029" },
            new[] { "97356", "?" },
            new[] { "3906", "895" },
            new[] { "245", "798" },
            new[] { "2=112--220-=-00=-=20" }
        };

        for (int day = 22; day <= 22; day++)
        {
            stopwatch.Restart();
            Console.WriteLine("Day " + day.ToString("00") + ":");
            string[] lines = File.ReadAllLines(Path.Combine("Input", "Day" + day.ToString("00") + ".txt"));
            days[day - 1](lines);
            stopwatch.Stop();
            Console.WriteLine();
            Console.WriteLine($"Solution time: {stopwatch.ElapsedMilliseconds} ms");
            if (yourPuzzleAnswers[day - 1].Length == 1)
            {
                Console.WriteLine($"Your puzzle answer was {yourPuzzleAnswers[day - 1][0]}");
            }
            else
            {
                Console.WriteLine($"Your puzzle answers were {string.Join(" and ", yourPuzzleAnswers[day - 1])}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    static int[,] ToGrid(string[] lines, Func<char, int> conversion)
    {
        int[,] grid = new int[lines.Length, lines[0].Length];
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                grid[y, x] = conversion(lines[y][x]);
            }
        }

        return grid;
    }

    static void Day01(string[] lines)
    {
        static IEnumerable<int> totals(string[] values)
        {
            int sum = 0;
            foreach (string s in values)
            {
                if (s.Length == 0)
                {
                    yield return sum;
                    sum = 0;
                }
                else
                {
                    sum += int.Parse(s);
                }
            }
            yield return sum;
        }

        Console.WriteLine("Most Calories total: " + totals(lines).Max());
        Console.WriteLine("Sum of top three Calorie totals: " + totals(lines).OrderDescending().Take(3).Sum());
    }

    static void Day02(string[] lines)
    {
        static int To_1_rock_2_paper_3_scissors(char c) => c switch { 'A' => 1, 'B' => 2, 'C' => 3, 'X' => 1, 'Y' => 2, 'Z' => 3, _ => 1 };
        static int To_0_lose_1_tie_2_win(char c) => c switch { 'X' => 0, 'Y' => 1, 'Z' => 2, _ => 0 };
        static int RockPaperScissorsResult(int yourMove, int opponentMove) => (yourMove - opponentMove + 4) % 3;
        static int YourMove(int opponentMove, int result) => (opponentMove + result + 1) % 3 + 1;
        int score1 = 0;
        int score2 = 0;
        foreach (string line in lines)
        {
            int opponentMove = To_1_rock_2_paper_3_scissors(line[0]);
            int yourPart1Move = To_1_rock_2_paper_3_scissors(line[2]);
            int part2Result = To_0_lose_1_tie_2_win(line[2]);
            score1 += yourPart1Move + 3 * RockPaperScissorsResult(yourPart1Move, opponentMove);
            score2 += YourMove(opponentMove, part2Result) + 3 * part2Result;
        }

        Console.WriteLine("Part 1 score: " + score1);
        Console.WriteLine("Part 2 score: " + score2);
    }

    static void Day03(string[] lines)
    {
        static char CommonCharacterInHalves(string s)
        {
            return s[..(s.Length / 2)].Intersect(s[(s.Length / 2)..]).Single();
        }

        static char CommonCharacterInGroup(IEnumerable<string> strings)
        {
            IEnumerator<string> enumerator = strings.GetEnumerator();
            enumerator.MoveNext();
            IEnumerable<char> common = enumerator.Current;
            while (enumerator.MoveNext())
            {
                common = common.Intersect(enumerator.Current);
            }

            return common.Single();
        }

        static int PriorityValue(char c) => c switch
        {
            >= 'a' and <= 'z' => c - 'a' + 1,
            >= 'A' and <= 'Z' => c - 'A' + 27,
            _ => 0
        };

        Console.WriteLine("Priority sum of common items: " + lines.Select(CommonCharacterInHalves).Sum(PriorityValue));
        Console.WriteLine("Priority sum of badges: " + lines.Chunk(3).Select(CommonCharacterInGroup).Sum(PriorityValue));
    }

    static void Day04(string[] lines)
    {
        static (Range, Range) Ranges(string s)
        {
            int i = s.IndexOf(',');
            return (new(s[..i]), new(s[(i + 1)..]));
        }

        Console.WriteLine("Count of pairs where a range fully contains the other: " + lines.Select(Ranges).Count(r => r.Item1.FullyContains(r.Item2) || r.Item2.FullyContains(r.Item1)));
        Console.WriteLine("Count of overlapping range pairs: " + lines.Select(Ranges).Count(r => r.Item1.Overlaps(r.Item2)));
    }

    static void Day05(string[] lines)
    {
        IEnumerable<string> cargoLines = lines.Where(s => s.StartsWith('[') || s.StartsWith(' '));
        IEnumerable<int> stackIndices = Enumerable.Range(0, cargoLines.Last().Length).Where(i => cargoLines.Last()[i] != ' ');
        List<List<char>> cargo1 = stackIndices.Select(i => cargoLines.Reverse().Select(s => s[i]).Where(c => c != ' ').ToList()).ToList();
        List<List<char>> cargo2 = stackIndices.Select(i => cargoLines.Reverse().Select(s => s[i]).Where(c => c != ' ').ToList()).ToList();

        foreach (string line in lines.Where(s => s.StartsWith("move")))
        {
            string[] tokens = line.Split(' ');
            int quantity = int.Parse(tokens[1]);
            int source = int.Parse(tokens[3]) - 1;
            int destination = int.Parse(tokens[5]) - 1;
            cargo1[destination].AddRange(cargo1[source].Skip(cargo1[source].Count - quantity).Reverse());
            cargo2[destination].AddRange(cargo2[source].Skip(cargo2[source].Count - quantity));
            cargo1[source].RemoveRange(cargo1[source].Count - quantity, quantity);
            cargo2[source].RemoveRange(cargo2[source].Count - quantity, quantity);
        }

        Console.WriteLine();
        Console.WriteLine("Part 1:");
        cargo1.ForEach(stack => Console.WriteLine(string.Join(null, stack)));
        Console.WriteLine();
        Console.WriteLine("Part 2:");
        cargo2.ForEach(stack => Console.WriteLine(string.Join(null, stack)));
    }

    static void Day06(string[] lines)
    {
        static int CountOfCharactersToDetectMarker(string buffer, int markerLength)
        {
            Dictionary<char, int> characterCounts = new();
            int i;
            for (i = 0; i < markerLength; i++)
            {
                characterCounts.Increment(buffer[i]);
            }

            for (; characterCounts.Values.Any(v => v > 1); i++)
            {
                characterCounts.Decrement(buffer[i - markerLength]);
                characterCounts.Increment(buffer[i]);
            }

            return i;
        }

        Console.WriteLine("Processed characters to obtain marker length 4: " + CountOfCharactersToDetectMarker(lines[0], 4));
        Console.WriteLine("Processed characters to obtain marker length 14: " + CountOfCharactersToDetectMarker(lines[0], 14));
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
        int[,] heightMap = ToGrid(lines, c => c - '0');
        int visibleTreeCount = 0;
        int maxScenicScore = 0;
        Vector2Int bounds = new(heightMap.GetLength(1), heightMap.GetLength(0));
        for (Vector2Int location = Vector2Int.Zero; location.Y < bounds.Y; location.Y++)
        {
            for (location.X = 0; location.X < bounds.X; location.X++)
            {
                bool visibility = false;
                int scenicScore = 1;
                foreach (Vector2Int direction in Vector2Int.Directions)
                {
                    bool directionVisibility = true;
                    int directionScenicScore = 0;
                    for (Vector2Int scan = location + direction; scan >= Vector2Int.Zero && scan < bounds; scan += direction)
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
            Vector2Int direction = line[0] switch { 'U' => Vector2Int.Up, 'D' => Vector2Int.Down, 'L' => Vector2Int.Left, 'R' => Vector2Int.Right, _ => Vector2Int.Zero };
            int moveCount = int.Parse(line[2..]);
            for (int move = 0; move < moveCount; move++)
            {
                rope[0] += direction;
                knotHistories[0].Add(rope[0]);
                for (int knot = 1; knot < rope.Length; knot++)
                {
                    Vector2Int difference = rope[knot - 1] - rope[knot];
                    Vector2Int absoluteDifference = difference.Abs();
                    if (absoluteDifference.X > 1 || absoluteDifference.Y > 1)
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
        static long Product(IEnumerable<int> ints)
        {
            long product = 1;
            foreach (int i in ints)
            {
                product *= i;
            }

            return product;
        }

        static long CalculateMonkeyBusinessLevel() => Product(Monkey.Troop.Select(m => m.TimesInspectedItems).OrderDescending().Take(2));

        Monkey.Troop.AddRange(lines.Chunk(7).Select(chunk => new Monkey(chunk)));
        for (int t = 0; t < 20; t++)
            Monkey.Troop.ForEach(m => m.TakeTurn(w => w / 3));
        Console.WriteLine("Level of monkey business (Part 1): " + CalculateMonkeyBusinessLevel());

        long productOfModuloDivisors = Product(Monkey.Troop.Select(m => m.ModuloDivisor));
        Monkey.Troop.ForEach(m => m.RevertToInitialCollection());
        for (int t = 0; t < 10000; t++)
            Monkey.Troop.ForEach(m => m.TakeTurn(w => w % productOfModuloDivisors));
        Console.WriteLine("Level of monkey business (Part 2): " + CalculateMonkeyBusinessLevel());
    }

    static void Day12(string[] lines)
    {
        int MinimumStepCountFromStartToEnd(int[,] heightMap, Vector2Int start, Vector2Int end)
        {
            HashSet<Vector2Int> open = new() { start };
            HashSet<Vector2Int> closed = new();
            Dictionary<Vector2Int, int> gCost = new() { { start, 0 } };
            int hCost(Vector2Int v) => Vector2Int.ManhattanDistance(v, end);
            int fCost(Vector2Int v) => gCost[v] + hCost(v);
            Vector2Int bounds = new(heightMap.GetLength(1), heightMap.GetLength(0));

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
                    if (neighbor >= Vector2Int.Zero && neighbor < bounds && heightMap[neighbor.Y, neighbor.X] <= currentHeight + 1 && !closed.Contains(neighbor))
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

        int MinimumStepCountFromHeightToEnd(int[,] heightMap, int startHeight, Vector2Int end)
        {
            HashSet<Vector2Int> open = new() { end };
            HashSet<Vector2Int> closed = new();
            Dictionary<Vector2Int, int> gCost = new() { { end, 0 } };
            Vector2Int bounds = new(heightMap.GetLength(1), heightMap.GetLength(0));

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
                    if (neighbor >= Vector2Int.Zero && neighbor < bounds && heightMap[neighbor.Y, neighbor.X] >= currentHeight - 1 && !closed.Contains(neighbor))
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

        int[,] heightMap = ToGrid(lines, c => c switch { 'S' => 1, 'E' => 26, _ => c - 'a' + 1 });
        int row = Enumerable.Range(0, lines.Length).First(i => lines[i].Contains('S'));
        int column = lines[row].IndexOf('S');
        Vector2Int start = new(column, row);
        row = Enumerable.Range(0, lines.Length).First(i => lines[i].Contains('E'));
        column = lines[row].IndexOf('E');
        Vector2Int end = new(column, row);
        Console.WriteLine("Step count (Part 1): " + MinimumStepCountFromStartToEnd(heightMap, start, end));
        Console.WriteLine("Step count (Part 2): " + MinimumStepCountFromHeightToEnd(heightMap, 1, end));
    }

    static void Day13(string[] lines)
    {
        static bool IsInCorrectOrder(string a, string b)
        {
            List<string> aTokens = BracketOrIntegerRegex().Matches(a).Select(m => m.Value).ToList();
            List<string> bTokens = BracketOrIntegerRegex().Matches(b).Select(m => m.Value).ToList();
            for (int i = 0; i < aTokens.Count && i < bTokens.Count; i++)
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

        RangeList sensedRangeList = new();
        void updateRangeList(int row)
        {
            sensedRangeList.Clear();
            for (int i = 0; i < sensorLocations.Length; i++)
            {
                int rowDistance = Math.Abs(row - sensorLocations[i].Y);
                int possibleColumnDistance = distances[i] - rowDistance;
                if (possibleColumnDistance >= 0)
                {
                    sensedRangeList.Add(new Range(sensorLocations[i].X - possibleColumnDistance, sensorLocations[i].X + possibleColumnDistance));
                }
            }
        }

        bool useSmallSearchSpace = sensorLocations.All(s => s.X <= 20 && s.Y <= 20);
        int targetRow = useSmallSearchSpace ? 10 : 2000000;
        updateRangeList(targetRow);
        int positionCount = sensedRangeList.Count - beaconLocations.Distinct().Count(b => b.Y == targetRow && sensedRangeList.Contains(b.X));
        Console.WriteLine($"Count of positions in row {targetRow} known not to have a beacon: {positionCount}");
        for (int row = useSmallSearchSpace ? 20 : 4000000; row >= 0; row--)
        {
            updateRangeList(row);
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
        HashSet<Vector3Int> cubes = new(lines.Select(s => new Vector3Int(s)));
        int surfaceArea = 0;
        foreach (Vector3Int cube in cubes)
        {
            foreach (Vector3Int direction in Vector3Int.Directions)
            {
                if (!cubes.Contains(cube + direction))
                {
                    surfaceArea++;
                }
            }
        }

        Console.WriteLine("Surface area (Part 1): " + surfaceArea);

        Vector3Int min = new Vector3Int(cubes.Select(v => v.X).Min() - 1, cubes.Select(v => v.Y).Min() - 1, cubes.Select(v => v.Z).Min() - 1);
        Vector3Int max = new Vector3Int(cubes.Select(v => v.X).Max() + 1, cubes.Select(v => v.Y).Max() + 1, cubes.Select(v => v.Z).Max() + 1);
        HashSet<Vector3Int> steam = new();
        Queue<Vector3Int> floodQueue = new();
        floodQueue.Enqueue(min);
        while (floodQueue.Count > 0)
        {
            Vector3Int current = floodQueue.Dequeue();
            if (min <= current && current <= max && !steam.Contains(current) && !cubes.Contains(current))
            {
                steam.Add(current);
                foreach (Vector3Int direction in Vector3Int.Directions)
                {
                    floodQueue.Enqueue(current + direction);
                }
            }
        }

        surfaceArea = 0;
        foreach (Vector3Int cube in cubes)
        {
            foreach (Vector3Int direction in Vector3Int.Directions)
            {
                if (steam.Contains(cube + direction))
                {
                    surfaceArea++;
                }
            }
        }

        Console.WriteLine("Surface area (Part 2): " + surfaceArea);
    }

    static void Day19(string[] lines)
    {
        Quarry quarry = new();
        quarry.Reset("Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.");

        quarry.EnqueueRobotBuildOrders(new[] { "clay", "clay", "clay", "obsidian", "clay", "obsidian", "geode", "geode" });
        for (int i = 0; i < 24; i++)
        {
            quarry.AdvanceMinute();
        }
    }

    static void Day20(string[] lines)
    {
        //int[] number = lines.Select(int.Parse).ToArray();
        int[] number = new int[] { 1, 2, -3, 3, -2, 0, 4 };
        int[] position = Enumerable.Range(0, number.Length).ToArray();
        int[] mixResult = new int[number.Length];

        Console.WriteLine(string.Join(", ", position.Select(p => number[p])));
        for (int i = 0; i < number.Length; i++)
        {
            int newPosition = position[i] + number[i];
            newPosition %= (number.Length - 1);
            if (newPosition <= 0)
            {
                newPosition += (number.Length - 1);
            }

            if (position[i] < newPosition)
            {
                for (int j = 0; j < position.Length; j++)
                {
                    if (position[i] < position[j] && position[j] <= newPosition)
                    {
                        position[j]--;
                    }
                }
            }
            else if (newPosition < position[i])
            {
                for (int j = 0; j < position.Length; j++)
                {
                    if (newPosition <= position[j] && position[j] < position[i])
                    {
                        position[j]++;
                    }
                }
            }

            position[i] = newPosition;

            for (int k = 0; k < number.Length; k++)
            {
                mixResult[position[k]] = number[k];
            }
            Console.WriteLine(string.Join(", ", mixResult));
        }

        int valueZeroPosition = position[Enumerable.Range(0, number.Length).First(i => number[i] == 0)];
        int groveCoordinate1 = number[Enumerable.Range(0, position.Length).First(i => position[i] == (valueZeroPosition + 1000) % number.Length)];
        int groveCoordinate2 = number[Enumerable.Range(0, position.Length).First(i => position[i] == (valueZeroPosition + 2000) % number.Length)];
        int groveCoordinate3 = number[Enumerable.Range(0, position.Length).First(i => position[i] == (valueZeroPosition + 3000) % number.Length)];
        Console.WriteLine("First grove coordinate: " + groveCoordinate1);
        Console.WriteLine("Second grove coordinate: " + groveCoordinate2);
        Console.WriteLine("Third grove coordinate: " + groveCoordinate3);
        Console.WriteLine("Sum: " + (groveCoordinate1 + groveCoordinate2 + groveCoordinate3));
    }

    static void Day21(string[] lines)
    {
        Dictionary<string, string> map = new();
        foreach (string line in lines)
        {
            string[] split = line.Split(": ");
            map.Add(split[0], split[1]);
        }

        long CalculateValue(string name)
        {
            if (long.TryParse(map[name], out long number))
                return number;

            string[] token = map[name].Split(' ');
            return token[1] switch
            {
                "+" => CalculateValue(token[0]) + CalculateValue(token[2]),
                "-" => CalculateValue(token[0]) - CalculateValue(token[2]),
                "*" => CalculateValue(token[0]) * CalculateValue(token[2]),
                "/" => CalculateValue(token[0]) / CalculateValue(token[2]),
                _ => 0,
            };
        }

        Console.WriteLine("root value: " + CalculateValue("root"));

        List<string> humanChain = new() { "humn" };
        while (humanChain.Last() != "root")
        {
            humanChain.Add(map.First(p => p.Value.Contains(humanChain.Last())).Key);
        }

        humanChain.Reverse();
        long valueToMatch = 0;
        for(int i = 0; i < humanChain.Count - 1; i++)
        {
            string[] currentTokens = map[humanChain[i]].Split(' ');
            if (humanChain[i] == "root")
            {
                if (humanChain[i + 1] == currentTokens[0])
                {
                    valueToMatch = CalculateValue(currentTokens[2]);
                }
                else if (humanChain[i + 1] == currentTokens[2])
                {
                    valueToMatch = CalculateValue(currentTokens[0]);
                }
            }
            else
            {
                if (humanChain[i + 1] == currentTokens[0])
                {
                    valueToMatch = currentTokens[1] switch
                    {
                        "+" => valueToMatch - CalculateValue(currentTokens[2]),
                        "-" => valueToMatch + CalculateValue(currentTokens[2]),
                        "*" => valueToMatch / CalculateValue(currentTokens[2]),
                        "/" => valueToMatch * CalculateValue(currentTokens[2]),
                        _ => valueToMatch
                    };
                
                }
                else if (humanChain[i + 1] == currentTokens[2])
                {
                    valueToMatch = currentTokens[1] switch
                    {
                        "+" => valueToMatch - CalculateValue(currentTokens[0]),
                        "-" => CalculateValue(currentTokens[0]) - valueToMatch,
                        "*" => valueToMatch / CalculateValue(currentTokens[0]),
                        "/" => CalculateValue(currentTokens[0]) / valueToMatch,
                        _ => valueToMatch
                    };
                }
            }
        }

        Console.WriteLine("humn value (Part 2): " + valueToMatch);
    }

    static void Day22(string[] lines)
    {
        Dictionary<(Vector2Int, Vector2Int), (Vector2Int, Vector2Int)> seams = new();
        if (lines.Length == 14)
        {
            Stitch(Vector2Int.Up, new Vector2Int(0, 4), new Vector2Int(7, 4), Vector2Int.Up, new Vector2Int(0, 7), new Vector2Int(7, 7));
            Stitch(Vector2Int.Up, new Vector2Int(8, 0), new Vector2Int(11, 0), Vector2Int.Up, new Vector2Int(8, 11), new Vector2Int(11, 11));
            Stitch(Vector2Int.Up, new Vector2Int(12, 8), new Vector2Int(16, 8), Vector2Int.Up, new Vector2Int(12, 11), new Vector2Int(16, 11));
            Stitch(Vector2Int.Left, new Vector2Int(8, 0), new Vector2Int(8, 3), Vector2Int.Left, new Vector2Int(11, 0), new Vector2Int(11, 3));
            Stitch(Vector2Int.Left, new Vector2Int(0, 4), new Vector2Int(0, 7), Vector2Int.Left, new Vector2Int(11, 4), new Vector2Int(11, 7));
            Stitch(Vector2Int.Left, new Vector2Int(8, 8), new Vector2Int(8, 11), Vector2Int.Left, new Vector2Int(15, 8), new Vector2Int(15, 11));
            WriteResults(FollowInstructions());

            seams.Clear();
            Stitch(Vector2Int.Up, new Vector2Int(0, 4), new Vector2Int(3, 4), Vector2Int.Down, new Vector2Int(11, 0), new Vector2Int(8, 0));
            Stitch(Vector2Int.Up, new Vector2Int(4, 4), new Vector2Int(7, 4), Vector2Int.Right, new Vector2Int(8, 0), new Vector2Int(8, 3));
            Stitch(Vector2Int.Up, new Vector2Int(12, 8), new Vector2Int(15, 8), Vector2Int.Left, new Vector2Int(11, 7), new Vector2Int(11, 4));
            Stitch(Vector2Int.Left, new Vector2Int(0, 4), new Vector2Int(0, 7), Vector2Int.Up, new Vector2Int(15, 11), new Vector2Int(12, 11));
            Stitch(Vector2Int.Left, new Vector2Int(8, 8), new Vector2Int(8, 11), Vector2Int.Up, new Vector2Int(7, 7), new Vector2Int(4, 7));
            Stitch(Vector2Int.Right, new Vector2Int(11, 0), new Vector2Int(11, 3), Vector2Int.Left, new Vector2Int(15, 11), new Vector2Int(15, 8));
            Stitch(Vector2Int.Down, new Vector2Int(0, 7), new Vector2Int(3, 7), Vector2Int.Up, new Vector2Int(11, 11), new Vector2Int(8, 11));
            WriteResults(FollowInstructions());
        }
        else if (lines.Length == 202)
        {
            /*
            Stitch(Vector2Int.Up, new Vector2Int(0, 100), new Vector2Int(49, 100), Vector2Int.Up, new Vector2Int(0, 199), new Vector2Int(49, 199));
            Stitch(Vector2Int.Up, new Vector2Int(50, 0), new Vector2Int(99, 0), Vector2Int.Up, new Vector2Int(50, 149), new Vector2Int(99, 149));
            Stitch(Vector2Int.Up, new Vector2Int(100, 0), new Vector2Int(149, 0), Vector2Int.Up, new Vector2Int(100, 49), new Vector2Int(149, 49));
            Stitch(Vector2Int.Left, new Vector2Int(50, 0), new Vector2Int(50, 49), Vector2Int.Left, new Vector2Int(149, 0), new Vector2Int(149, 49));
            Stitch(Vector2Int.Left, new Vector2Int(50, 50), new Vector2Int(50, 99), Vector2Int.Left, new Vector2Int(99, 50), new Vector2Int(99, 99));
            Stitch(Vector2Int.Left, new Vector2Int(0, 100), new Vector2Int(0, 149), Vector2Int.Left, new Vector2Int(99, 100), new Vector2Int(99, 149));
            Stitch(Vector2Int.Left, new Vector2Int(0, 150), new Vector2Int(0, 199), Vector2Int.Left, new Vector2Int(49, 150), new Vector2Int(49, 199));
            WriteResults(FollowInstructions());
            */

            seams.Clear();
            Stitch(Vector2Int.Up, new Vector2Int(50, 0), new Vector2Int(99, 0), Vector2Int.Right, new Vector2Int(0, 150), new Vector2Int(0, 199));
            Stitch(Vector2Int.Left, new Vector2Int(50, 0), new Vector2Int(50, 49), Vector2Int.Right, new Vector2Int(0, 149), new Vector2Int(0, 100));
            Stitch(Vector2Int.Up, new Vector2Int(100, 0), new Vector2Int(149, 0), Vector2Int.Up, new Vector2Int(0, 199), new Vector2Int(49, 199));
            Stitch(Vector2Int.Right, new Vector2Int(149, 0), new Vector2Int(149, 49), Vector2Int.Left, new Vector2Int(99, 149), new Vector2Int(99, 100));
            Stitch(Vector2Int.Down, new Vector2Int(100, 49), new Vector2Int(149, 49), Vector2Int.Left, new Vector2Int(99, 50), new Vector2Int(99, 99));
            Stitch(Vector2Int.Left, new Vector2Int(50, 50), new Vector2Int(50, 99), Vector2Int.Down, new Vector2Int(49, 100), new Vector2Int(0, 100));
            Stitch(Vector2Int.Down, new Vector2Int(50, 149), new Vector2Int(99, 149), Vector2Int.Left, new Vector2Int(49, 150), new Vector2Int(49, 199));
            // range [64688 to 123114], excluding 99303
            WriteResults(FollowInstructions());
        }

        void Stitch(Vector2Int edge1Direction, Vector2Int edge1Start, Vector2Int edge1End, Vector2Int edge2Direction, Vector2Int edge2Start, Vector2Int edge2End)
        {
            (Vector2Int direction, Vector2Int location) edge1 = (edge1Direction, edge1Start);
            (Vector2Int direction, Vector2Int location) edge2 = (edge2Direction, edge2Start);
            Vector2Int edge1Increment = (edge1End - edge1Start).Sign();
            Vector2Int edge2Increment = (edge2End - edge2Start).Sign();
            while (true)
            {
                /*
                Console.Write(edge1);
                Console.Write(" & ");
                Console.Write(edge2);
                Console.Write("    ");
                */
                seams[edge1] = edge2;
                seams[(-edge2.direction, edge2.location)] = (-edge1.direction, edge1.location);
                if (edge1.location == edge1End || edge2.location == edge2End)
                {
                    break;
                }

                edge1.location += edge1Increment;
                edge2.location += edge2Increment;
            }
        }

        (Vector2Int direction, Vector2Int location) FollowInstructions()
        {
            Vector2Int start = new(lines[0].IndexOf('.'), 0);
            (Vector2Int direction, Vector2Int location) current = (Vector2Int.Right, start);
            foreach (string token in WholeNumberOrRLRegex().Matches(lines[^1]).Select(m => m.Value))
            {
                if (token == "R")
                    current.direction.TurnRight();
                else if (token == "L")
                    current.direction.TurnLeft();
                else
                {
                    int steps = int.Parse(token);
                    for (int i = 0; i < steps; i++)
                    {
                        if (!seams.TryGetValue(current, out (Vector2Int direction, Vector2Int location) next))
                            next = (current.direction, current.location + current.direction.WithInvertedY());

                        if (lines[next.location.Y][next.location.X] == '#')
                            break;
                        //Console.Write(next.location);
                        //Console.Write("   ");
                        current = next;
                    }
                }
            }

            return current;
        }

        void WriteResults((Vector2Int direction, Vector2Int location) current)
        {
            Console.WriteLine("Ending row: " + (current.location.Y + 1));
            Console.WriteLine("Ending column: " + (current.location.X + 1));
            Console.WriteLine("Ending direction: " + current.direction.ToChar());
            int password = (current.location.Y + 1) * 1000 + (current.location.X + 1) * 4 + ">v<^".IndexOf(current.direction.ToChar());
            Console.WriteLine("Password: " + password);
            Console.WriteLine();
        }
    }

    static void Day23(string[] lines)
    {
        List<Vector2Int> elfLocations = new();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[^(y+1)].Length; x++)
            {
                if (lines[^(y + 1)][x] == '#')
                {
                    elfLocations.Add(new(x, y));
                }
            }
        }

        HashSet<Vector2Int> elfLocationsHashSet = new();
        Vector2Int[] proposalDirections = { Vector2Int.Up, Vector2Int.Down, Vector2Int.Left, Vector2Int.Right };
        Dictionary<Vector2Int, int> timesLocationProposed = new();
        Dictionary<Vector2Int, int> lastElfThatProposedLocation = new();
        int round = 0;
        do
        {
            round++;
            timesLocationProposed.Clear();
            elfLocationsHashSet.Clear();
            elfLocationsHashSet.UnionWith(elfLocations);

            for (int i = 0; i < elfLocations.Count; i++)
            {
                Vector2Int n = new(elfLocations[i].X, elfLocations[i].Y + 1);
                Vector2Int e = new(elfLocations[i].X + 1, elfLocations[i].Y);
                Vector2Int s = new(elfLocations[i].X, elfLocations[i].Y - 1);
                Vector2Int w = new(elfLocations[i].X - 1, elfLocations[i].Y);
                Vector2Int ne = new(e.X, n.Y);
                Vector2Int se = new(e.X, s.Y);
                Vector2Int sw = new(w.X, s.Y);
                Vector2Int nw = new(w.X, n.Y);

                byte occupation = 0;
                if (elfLocationsHashSet.Contains(n))
                    occupation |= 0b1000_0000;

                if (elfLocationsHashSet.Contains(ne))
                    occupation |= 0b0100_0000;

                if (elfLocationsHashSet.Contains(e))
                    occupation |= 0b0010_0000;

                if (elfLocationsHashSet.Contains(se))
                    occupation |= 0b0001_0000;

                if (elfLocationsHashSet.Contains(s))
                    occupation |= 0b0000_1000;

                if (elfLocationsHashSet.Contains(sw))
                    occupation |= 0b0000_0100;

                if (elfLocationsHashSet.Contains(w))
                    occupation |= 0b0000_0010;

                if (elfLocationsHashSet.Contains(nw))
                    occupation |= 0b0000_0001;

                if (occupation != 0)
                {
                    for (int d = 0; d < 4; d++)
                    {
                        Vector2Int proposalDirection = proposalDirections[(round - 1 + d) % 4];
                        if (proposalDirection == Vector2Int.Up)
                        {
                            if ((occupation & 0b1100_0001) == 0)
                            {
                                timesLocationProposed.Increment(n);
                                lastElfThatProposedLocation[n] = i;
                                break;
                            }
                        }
                        else if (proposalDirection == Vector2Int.Down)
                        {
                            if ((occupation & 0b0001_1100) == 0)
                            {
                                timesLocationProposed.Increment(s);
                                lastElfThatProposedLocation[s] = i;
                                break;
                            }
                        }
                        else if (proposalDirection == Vector2Int.Left)
                        {
                            if ((occupation & 0b0000_0111) == 0)
                            {
                                timesLocationProposed.Increment(w);
                                lastElfThatProposedLocation[w] = i;
                                break;
                            }
                        }
                        else if (proposalDirection == Vector2Int.Right)
                        {
                            if ((occupation & 0b0111_0000) == 0)
                            {
                                timesLocationProposed.Increment(e);
                                lastElfThatProposedLocation[e] = i;
                                break;
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<Vector2Int, int> pair in timesLocationProposed.Where(p => p.Value == 1))
            {
                elfLocations[lastElfThatProposedLocation[pair.Key]] = pair.Key;
            }

            if (round == 10)
            {
                Vector2Int rectangleMin = new(elfLocations.Select(v => v.X).Min(), elfLocations.Select(v => v.Y).Min());
                Vector2Int rectangleMax = new(elfLocations.Select(v => v.X).Max(), elfLocations.Select(v => v.Y).Max());

                int emptyGroundTiles = (rectangleMax - rectangleMin + Vector2Int.One).Area() - elfLocations.Count;
                Console.WriteLine($"Empty ground tiles in rectangle after {round} round(s): {emptyGroundTiles}");
            }

        } while (timesLocationProposed.Count > 0);

        Console.WriteLine("First round where no elf moved " + round);
    }

    static void Day24(string[] lines)
    {
        int GreatestCommonDenominator(int a, int b)
        {
            do
            {
                int r = a % b;
                a = b;
                b = r;
            } while (b != 0);

            return a;
        }

        int LeastCommonMultiple(int a, int b) => a * b / GreatestCommonDenominator(a, b);

        int width = lines[0].Length - 2;
        int height = lines.Length - 2;
        byte[,,] field = new byte[LeastCommonMultiple(width, height), lines.Length, lines[0].Length];
        const byte wall = 0b11110000;
        const byte blizzardN = 0b1000;
        const byte blizzardS = 0b0100;
        const byte blizzardW = 0b0010;
        const byte blizzardE = 0b0001;

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                field[0, y, x] = (lines[^(y + 1)][x]) switch
                {
                    '#' => wall,
                    '^' => blizzardN,
                    'v' => blizzardS,
                    '<' => blizzardW,
                    '>' => blizzardE,
                    '.' => 0,
                    _ => 0
                };
            }
        }

        for (int z = 0; z < field.GetLength(0) - 1; z++)
        {
            for (int y = 0; y < field.GetLength(1); y++)
            {
                for (int x = 0; x < field.GetLength(2); x++)
                {
                    if ((field[z, y, x] & wall) != 0)
                    {
                        field[z + 1, y, x] = wall;
                    }

                    if ((field[z, y, x] & blizzardN) != 0)
                    {
                        if (y == height)
                        {
                            field[z + 1, 1, x] |= blizzardN;
                        }
                        else
                        {
                            field[z + 1, y + 1, x] |= blizzardN;
                        }
                    }

                    if ((field[z, y, x] & blizzardS) != 0)
                    {
                        if (y == 1)
                        {
                            field[z + 1, height, x] |= blizzardS;
                        }
                        else
                        {
                            field[z + 1, y - 1, x] |= blizzardS;
                        }
                    }

                    if ((field[z, y, x] & blizzardW) != 0)
                    {
                        if (x == 1)
                        {
                            field[z + 1, y, width] |= blizzardW;
                        }
                        else
                        {
                            field[z + 1, y, x - 1] |= blizzardW;
                        }
                    }

                    if ((field[z, y, x] & blizzardE) != 0)
                    {
                        if (x == width)
                        {
                            field[z + 1, y, 1] |= blizzardE;
                        }
                        else
                        {
                            field[z + 1, y, x + 1] |= blizzardE;
                        }
                    }
                }
            }
        }

        int MinimumStepCount(Vector3Int start, Vector3Int end)
        {
            HashSet<Vector3Int> open = new() { start };
            HashSet<Vector3Int> closed = new();
            Dictionary<Vector3Int, int> gCost = new() { { start, 0 } };
            int hCost(Vector3Int v) => Vector2Int.ManhattanDistance(new Vector2Int(v.X, v.Y), new Vector2Int(end.X, end.Y));
            int fCost(Vector3Int v) => gCost[v] + hCost(v);
            Vector3Int bounds = new(field.GetLength(2), field.GetLength(1), field.GetLength(0));
            Vector3Int[] directions = new Vector3Int[] {
                new Vector3Int(0, 0, 1),
                new Vector3Int(1, 0, 1),
                new Vector3Int(-1, 0, 1),
                new Vector3Int(0, -1, 1),
                new Vector3Int(0, 1, 1)
            };

            while (true)
            {
                Vector3Int current = open.MinBy(v => fCost(v));
                open.Remove(current);
                closed.Add(current);

                if (current.X == end.X && current.Y == end.Y)
                    return gCost[current];

                foreach (Vector3Int direction in directions)
                {
                    Vector3Int neighbor = current + direction;
                    neighbor.Z %= bounds.Z;

                    if (neighbor >= Vector3Int.Zero && neighbor < bounds && field[neighbor.Z, neighbor.Y, neighbor.X] == 0 && !closed.Contains(neighbor))
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

        Vector3Int start = new(1, height + 1, 0);
        Vector3Int end = new(width, 0, 0);
        int trip1minutes = MinimumStepCount(start, end);
        Console.WriteLine("Minutes from start to end: " + trip1minutes);
        start.Z += trip1minutes;
        end.Z += trip1minutes;
        int trip2minutes = MinimumStepCount(end, start);
        start.Z += trip2minutes;
        end.Z += trip2minutes;
        int trip3minutes = MinimumStepCount(start, end);
        Console.WriteLine("Minutes from start to end to start to end: " + (trip1minutes + trip2minutes + trip3minutes));
    }

    static void Day25(string[] lines)
    {
        Snafu snafu = new();
        foreach (string line in lines)
        {
            snafu.Add(new Snafu(line));
        }

        Console.WriteLine("SNAFU sum: " + snafu);
    }
}
