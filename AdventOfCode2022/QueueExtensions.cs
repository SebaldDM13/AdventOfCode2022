namespace AdventOfCode2022;

public static class QueueExtensions
{
    public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            queue.Enqueue(item);
        }
    }
}
