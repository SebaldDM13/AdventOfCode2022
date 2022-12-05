public class CrateMover9001 : CrateMover
{
    public override void Move(int count, Stack<char> source, Stack<char> destination)
    {
        Stack<char> temporaryStack = new();
        for (int i = 0; i < count; i++)
        {
            temporaryStack.Push(source.Pop());
        }

        for (int i = 0; i < count; i++)
        {
            destination.Push(temporaryStack.Pop());
        }
    }
}
