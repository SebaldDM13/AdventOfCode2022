public class CrateMover9000 : CrateMover
{
    public override void Move(int count, Stack<char> source, Stack<char> destination)
    {
        for (int i = 0; i < count; i++)
        {
            destination.Push(source.Pop());
        }
    }
}
