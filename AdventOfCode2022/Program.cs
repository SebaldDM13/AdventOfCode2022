using AdventOfCode2022;
using System.Xml.Schema;

const string path = "Input";
IEnumerable<string> lines;
Console.WriteLine("Advent of Code 2022");
Console.WriteLine();

Console.WriteLine("Day 01:");
lines = File.ReadLines(Path.Combine(path, "Day01.txt"));
Console.WriteLine("Most Calories total: " + lines.Partitions().Totals().Max());
Console.WriteLine("Sum of top three Calorie totals: " + lines.Partitions().Totals().OrderDescending().Take(3).Sum());
Console.WriteLine();

Console.WriteLine("Day 02:");
lines = File.ReadLines(Path.Combine(path, "Day02.txt"));
Console.WriteLine("Part 1 score (switch implementation): " + lines.Sum(RockPaperScissorsSwitchImplementation.ScoreOfOpponentMoveAndPlayerMove));
Console.WriteLine("Part 2 score (switch implementation): " + lines.Sum(RockPaperScissorsSwitchImplementation.ScoreOfOpponentMoveAndResult));
Console.WriteLine("Part 1 score (equation implementation): " + lines.Sum(RockPaperScissorsEquationImplementation.ScoreOfOpponentMoveAndPlayerMove));
Console.WriteLine("Part 2 score (equation implementation): " + lines.Sum(RockPaperScissorsEquationImplementation.ScoreOfOpponentMoveAndResult));
Console.WriteLine();

Console.WriteLine("Day 03:");
lines = File.ReadLines(Path.Combine(path, "Day03.txt"));
Console.WriteLine("Priority sum of common items: " + lines.Halves().CommonCharacters().Singles().PriorityValues().Sum());
Console.WriteLine("Priority sum of badges: " + lines.Chunk(3).CommonCharacters().Singles().PriorityValues().Sum());
