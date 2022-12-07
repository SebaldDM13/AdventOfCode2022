namespace AdventOfCode2022;

public class FileNode : INode
{
    public DirectoryNode? Parent { get; set; }
    public string Name { get; }
    public int Size { get; }

    public FileNode(string name, int size)
    {
        Name = name;
        Size = size;
    }
}
