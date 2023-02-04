using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022;

public struct Vector2Int : IEquatable<Vector2Int>
{
    public int X { get; set; }
    public int Y { get; set; }

    public Vector2Int(int x, int y)
    {
        (X, Y) = (x, y);
    }

    public Vector2Int(string s) : this(s.Split(',').Select(int.Parse))
    {
    }

    public Vector2Int(IEnumerable<int> coordinates)
    {
        IEnumerator<int> enumerator = coordinates.GetEnumerator();
        enumerator.MoveNext();
        X = enumerator.Current;
        enumerator.MoveNext();
        Y = enumerator.Current;
    }

    public Vector2Int Sign() => new(Math.Sign(X), Math.Sign(Y));
    public Vector2Int Abs() => new(Math.Abs(X), Math.Abs(Y));
    public void TurnRight() => (X, Y) = (Y, -X);
    public void TurnLeft() => (X, Y) = (-Y, X);
    public char ToChar() => (X, Y) switch { (0, 1) => '^', (0, -1) => 'v', (-1, 0) => '<', (1, 0) => '>', _ => '.' };
    public int Area() => X * Y;
    public Vector2Int WithInvertedY() => new(X, -Y);
    public static int ManhattanDistance(Vector2Int a, Vector2Int b) => Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y);
    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2Int operator -(Vector2Int a) => new(-a.X, -a.Y);
    public static bool operator <(Vector2Int lhs, Vector2Int rhs) => lhs.X < rhs.X && lhs.Y < rhs.Y;
    public static bool operator >(Vector2Int lhs, Vector2Int rhs) => lhs.X > rhs.X && lhs.Y > rhs.Y;
    public static bool operator <=(Vector2Int lhs, Vector2Int rhs) => lhs.X <= rhs.X && lhs.X <= rhs.Y;
    public static bool operator >=(Vector2Int lhs, Vector2Int rhs) => lhs.X >= rhs.X && lhs.Y >= rhs.Y;
    public static bool operator ==(Vector2Int lhs, Vector2Int rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
    public static bool operator !=(Vector2Int lhs, Vector2Int rhs) => !(lhs == rhs);
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector2Int v && Equals(v);
    public bool Equals(Vector2Int other) => X == other.X && Y == other.Y;
    public override int GetHashCode() => (X, Y).GetHashCode();
    public override string ToString() => string.Join(',', X, Y);
    public static Vector2Int Zero => zero;
    public static Vector2Int One => one;
    public static Vector2Int Up => up;
    public static Vector2Int Down => down;
    public static Vector2Int Left => left;
    public static Vector2Int Right => right;
    private static readonly Vector2Int zero = new(0, 0);
    private static readonly Vector2Int one = new(1, 1);
    private static readonly Vector2Int up = new(0, 1);
    private static readonly Vector2Int down = new (0, -1);
    private static readonly Vector2Int left = new(-1, 0);
    private static readonly Vector2Int right = new(1, 0);
    public static IEnumerable<Vector2Int> Directions
    {
        get
        {
            yield return up;
            yield return down;
            yield return left;
            yield return right;
        }
    }
}
