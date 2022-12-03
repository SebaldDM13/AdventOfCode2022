using AdventOfCode2022;

const string path = "Input";
IEnumerable<string> lines;
Console.WriteLine("Advent of Code 2022");
Console.WriteLine();
Console.WriteLine("Day 01:");
lines = File.ReadLines(Path.Combine(path, "Day01.txt"));
Console.WriteLine("Most Calories total: " + GroupAdder.CalculateTotals(lines, string.Empty).Max());
Console.WriteLine("Sum of top three Calorie totals: " + GroupAdder.CalculateTotals(lines, string.Empty).OrderDescending().Take(3).Sum());
Console.WriteLine();
Console.WriteLine("Day 02:");
lines = File.ReadLines(Path.Combine(path, "Day02.txt"));
Console.WriteLine("Part 1 score (switch implementation): " + lines.Select(line => RockPaperScissorsSwitchImplementation.ScoreOfOpponentMoveAndPlayerMove(line)).Sum());
Console.WriteLine("Part 2 score (switch implementation): " + lines.Select(line => RockPaperScissorsSwitchImplementation.ScoreOfOpponentMoveAndResult(line)).Sum());
Console.WriteLine("Part 1 score (equation implementation): " + lines.Select(line => RockPaperScissorsEquationImplementation.ScoreOfOpponentMoveAndPlayerMove(line)).Sum());
Console.WriteLine("Part 2 score (equation implementation): " + lines.Select(line => RockPaperScissorsEquationImplementation.ScoreOfOpponentMoveAndResult(line)).Sum());
