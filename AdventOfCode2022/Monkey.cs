namespace AdventOfCode2022;

public class Monkey
{
    public static List<Monkey> Troop { get; } = new();
    private readonly Queue<long> startingHeldItems = new();
    private readonly Queue<long> heldItems = new();
    private readonly char operation;
    private readonly string secondParameter = string.Empty;
    public readonly int moduloDivisor;
    private readonly int recipientIfTestResultTrue;
    private readonly int recipientIfTestResultFalse;

    public int ModuloDivisor => moduloDivisor;

    public int TimesInspectedItems { get; private set; }

    public Monkey(IEnumerable<string> lines)
    {
        const string startingItemsIndicator = "Starting items:";
        const string operationIndicator = "Operation: new = old ";
        const string testIndicator = "Test: divisible by ";
        const string trueIndicator = "If true: throw to monkey ";
        const string falseIndicator = "If false: throw to monkey ";

        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (trimmed.StartsWith(startingItemsIndicator))
            {
                startingHeldItems.EnqueueRange(trimmed[startingItemsIndicator.Length..].Split(',', StringSplitOptions.TrimEntries).Select(long.Parse));
            }
            else if (trimmed.StartsWith(operationIndicator))
            {
                operation = trimmed[operationIndicator.Length];
                secondParameter = trimmed[(operationIndicator.Length + 2)..];
            }
            else if (trimmed.StartsWith(testIndicator))
            {
                moduloDivisor = int.Parse(trimmed[testIndicator.Length..]);
            }
            else if (trimmed.StartsWith(trueIndicator))
            {
                recipientIfTestResultTrue = int.Parse(trimmed[trueIndicator.Length..]);
            }
            else if (trimmed.StartsWith(falseIndicator))
            {
                recipientIfTestResultFalse = int.Parse(trimmed[falseIndicator.Length..]);
            }
        }

        Reset();
    }

    public void Reset()
    {
        heldItems.Clear();
        heldItems.EnqueueRange(startingHeldItems);
        TimesInspectedItems = 0;
    }

    public void TakeTurn(Func<long, long> worryMitigation)
    {
        const string oldVariableString = "old";

        while (heldItems.Count > 0)
        {
            TimesInspectedItems++;
            long item = heldItems.Dequeue();
            long secondParameterValue = (secondParameter == oldVariableString ? item : long.Parse(secondParameter));

            if (operation == '+')
                item += secondParameterValue;
            else if (operation == '*')
                item *= secondParameterValue;

            item = worryMitigation(item);

            int recipient = (item % moduloDivisor == 0) ? recipientIfTestResultTrue : recipientIfTestResultFalse;
            Troop[recipient].heldItems.Enqueue(item);
        }
    }
}
