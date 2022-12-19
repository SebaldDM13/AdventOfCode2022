using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022;

public struct Vector3Int : IEquatable<Vector3Int>
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    public Vector3Int(int x, int y, int z)
    {
        (X, Y, Z) = (x, y, z);
    }

    public Vector3Int(string s) : this(s.Split(',').Select(int.Parse))
    {
    }
    
    public Vector3Int(IEnumerable<int> coordinates)
    {
        IEnumerator<int> enumerator = coordinates.GetEnumerator();
        enumerator.MoveNext();
        X = enumerator.Current;
        enumerator.MoveNext();
        Y = enumerator.Current;
        enumerator.MoveNext();
        Z = enumerator.Current;
    }

    public Vector3Int Sign() => new(Math.Sign(X), Math.Sign(Y), Math.Sign(Z));
    public Vector3Int Abs() => new(Math.Abs(X), Math.Abs(Y), Math.Abs(Z));
    public static int ManhattanDistance(Vector3Int a, Vector3Int b) => Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y) + Math.Abs(b.Z - a.Z);
    public static bool AreAdjacent(Vector3Int a, Vector3Int b) => ManhattanDistance(a, b) == 1;
    public static Vector3Int operator +(Vector3Int a, Vector3Int b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vector3Int operator -(Vector3Int a, Vector3Int b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static bool operator <(Vector3Int lhs, Vector3Int rhs) => lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z;
    public static bool operator >(Vector3Int lhs, Vector3Int rhs) => lhs.X > rhs.Y && lhs.Y > rhs.Y && lhs.Z > rhs.Z;
    public static bool operator <=(Vector3Int lhs, Vector3Int rhs) => lhs.X <= rhs.X && lhs.Y <= rhs.Y && lhs.Z <= rhs.Z;
    public static bool operator >=(Vector3Int lhs, Vector3Int rhs) => lhs.X >= rhs.X && lhs.Y >= rhs.Y && lhs.Z >= rhs.Z;
    public static bool operator ==(Vector3Int lhs, Vector3Int rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;
    public static bool operator !=(Vector3Int lhs, Vector3Int rhs) => !(lhs == rhs);
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector3Int v && Equals(v);
    public bool Equals(Vector3Int other) => X == other.X && Y == other.Y && Z == other.Z;
    public override int GetHashCode() => (X, Y, Z).GetHashCode();
    public override string ToString() => string.Join(',', X, Y, Z);
    public static Vector3Int Zero => zero;
    public static Vector3Int One => one;
    public static Vector3Int Up => up;
    public static Vector3Int Down => down;
    public static Vector3Int Left => left;
    public static Vector3Int Right => right;
    public static Vector3Int Back => back;
    public static Vector3Int Forward => forward;
    private static readonly Vector3Int zero = new(0, 0, 0);
    private static readonly Vector3Int one = new(1, 1, 1);
    private static readonly Vector3Int up = new(0, 1, 0);
    private static readonly Vector3Int down = new(0, -1, 0);
    private static readonly Vector3Int left = new(-1, 0, 0);
    private static readonly Vector3Int right = new(1, 0, 0);
    private static readonly Vector3Int back = new(0, 0, -1);
    private static readonly Vector3Int forward = new(0, 0, 1);
    public static IEnumerable<Vector3Int> Directions
    {
        get
        {
            yield return up;
            yield return down;
            yield return left;
            yield return right;
            yield return back;
            yield return forward;
        }
    }
}
