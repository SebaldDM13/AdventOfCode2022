namespace AdventOfCode2022;

public class DirectoryNode : INode
{
    public DirectoryNode? Parent { get; set; }
    public string Name { get; }
    public int Size => children.Sum(c => c.Size);

    private readonly List<INode> children = new();

    public DirectoryNode(string name)
    {
        Name = name;
    }

    public void Add(INode component)
    {
        component.Parent = this;
        children.Add(component);
    }

    public DirectoryNode SubDirectory(string name)
    {
        return SubDirectories().Single(d => d.Name == name);
    }

    public IEnumerable<DirectoryNode> SubDirectories()
    {
        return children.OfType<DirectoryNode>();
    }
}
