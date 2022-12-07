namespace AdventOfCode2022;

public interface INode
{
    DirectoryNode? Parent { get; set; }
    string Name { get; }
    int Size { get; }
}
