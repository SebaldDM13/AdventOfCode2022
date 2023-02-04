namespace AdventOfCode2022;

public class Quarry
{
    private int minute = 0;
    private int blueprintNumber = 0;
    private readonly Dictionary<string, Dictionary<string, int>> blueprint = new();
    private readonly Dictionary<string, int> robotInventory = new();
    private readonly Dictionary<string, int> materialInventory = new();
    public readonly Queue<string> robotBuildQueue = new();

    public Quarry()
    {
    }

    public void Reset(string blueprintString)
    {
        minute = 0;
        robotInventory.Clear();
        robotInventory["ore"] = 1;
        materialInventory.Clear();
        materialInventory["ore"] = 0;
        materialInventory["clay"] = 0;
        materialInventory["obsidian"] = 0;
        materialInventory["geode"] = 0;
        robotBuildQueue.Clear();
        ParseBlueprintString(blueprintString);
    }

    public int Count(string material)
    {
        return materialInventory[material];
    }

    private void ParseBlueprintString(string blueprintString)
    {
        blueprintNumber = 0;
        blueprint.Clear();

        string[] word = blueprintString.Split(new char[] { ' ', '.', ':' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string robot = string.Empty;
        for (int i = 0; i < word.Length; i++)
        {
            switch (word[i])
            {
                case "Blueprint":
                    blueprintNumber = int.Parse(word[i + 1]);
                    i++;
                    break;
                case "robot":
                    robot = word[i - 1];
                    blueprint[robot] = new();
                    break;
                case "costs":
                case "and":
                    blueprint[robot].Increase(word[i + 2], int.Parse(word[i + 1]));
                    i += 2;
                    break;
                default:
                    break;
            }
        }

        Console.WriteLine("Blueprint " + blueprintNumber);
    }

    public void EnqueueRobotBuildOrders(IEnumerable<string> orders)
    {
        robotBuildQueue.EnqueueRange(orders);
    }

    public void AdvanceMinute()
    {
        Console.WriteLine($"Minute {minute + 1}");
        string robotToBuild = string.Empty;
        if (robotBuildQueue.Count > 0 && materialInventory.Satisfies(blueprint[robotBuildQueue.Peek()]))
        {
            robotToBuild = robotBuildQueue.Dequeue();
            materialInventory.Decrease(blueprint[robotToBuild]);
            Console.WriteLine("Withdrew materials to build " + robotToBuild + " robot");
        }

        materialInventory.Increase(robotInventory);
        Console.WriteLine($"After collecting, total is {materialInventory["ore"]} ore, {materialInventory["clay"]} clay, {materialInventory["obsidian"]} obsidian, {materialInventory["geode"]} geode");

        if (robotToBuild.Length > 0)
        {
            robotInventory.Increment(robotToBuild);
            Console.WriteLine("Build complete");
        }

        minute++;
        Console.WriteLine();
    }
}
