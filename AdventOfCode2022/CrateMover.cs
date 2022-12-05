public abstract class CrateMover
{
    public void Move(Stack<char>[] cargo, string instruction)
    {
        string[] tokens = instruction.Split(' ');
        int count = int.Parse(tokens[1]);
        int source = int.Parse(tokens[3]) - 1;
        int destination = int.Parse(tokens[5]) - 1;
        Move(count, cargo[source], cargo[destination]);
    }

    public abstract void Move(int count, Stack<char> source, Stack<char> destination);
}
