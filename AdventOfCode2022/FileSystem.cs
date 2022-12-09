namespace AdventOfCode2022;

public class FileSystem
{
    private const string PATH_SEPARATOR = "/";

    public DirectoryNode Root { get; } = new(PATH_SEPARATOR);

    public FileSystem(IEnumerable<string> lines)
    {
        DirectoryNode current = Root;
        bool isInListing = false;

        foreach (string line in lines)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens[0] == "$")
            {
                isInListing = tokens[1] == "ls";
                if (tokens[1] == "cd")
                {
                    current = tokens[2] switch
                    {
                        PATH_SEPARATOR => Root,
                        ".." => (current.Parent is not null) ? current.Parent : current,
                        _ => current.SubDirectory(tokens[2])
                    };
                }
            }
            else if (isInListing)
            {
                current.Add(tokens[0] == "dir" ? new DirectoryNode(tokens[1]) : new FileNode(tokens[1], int.Parse(tokens[0])));
            }
        }
    }

    public IEnumerable<DirectoryNode> DirectoryTraversal()
    {
        Queue<DirectoryNode> queue = new();
        queue.Enqueue(Root);
        while (queue.Count > 0)
        {
            DirectoryNode current = queue.Dequeue();
            foreach (DirectoryNode subDirectory in current.SubDirectories())
            {
                queue.Enqueue(subDirectory);
            }

            yield return current;
        }
    }

}
