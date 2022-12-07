namespace AdventOfCode2022;

public class FileSystem
{
    public List<DirectoryNode> AllDirectories { get; } = new List<DirectoryNode>();

    public void Initialize(IEnumerable<string> lines)
    {
        AllDirectories.Clear();
        AllDirectories.Add(new("/"));
        DirectoryNode current = AllDirectories[0];
        bool isInListing = false;

        foreach (string line in lines)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens[0] == "$")
            {
                isInListing = false;
                if (tokens[1] == "ls")
                {
                    isInListing = true;
                }
                if (tokens[1] == "cd")
                {
                    if (tokens[2] == "/")
                    {
                        current = AllDirectories[0];
                    }
                    else if (tokens[2] == "..")
                    {
                        if (current.Parent is not null)
                        {
                            current = current.Parent;
                        }
                    }
                    else
                    {
                        current = current.SubDirectory(tokens[2]);
                    }
                }
            }
            else if (isInListing)
            {
                if (tokens[0] == "dir")
                {
                    DirectoryNode subDirectory = new DirectoryNode(tokens[1]);
                    current.Add(subDirectory);
                    AllDirectories.Add(subDirectory);
                }
                else
                {
                    current.Add(new FileNode(tokens[1], int.Parse(tokens[0])));
                }
            }
        }
    }
}
