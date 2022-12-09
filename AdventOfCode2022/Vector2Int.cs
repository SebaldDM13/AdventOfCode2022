
// based on https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Math/Vector2Int.cs
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2022;

public struct Vector2Int : IEquatable<Vector2Int>
{
    public int X { get; set; }
    public int Y { get; set; }

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Vector2Int Sign() => new (Math.Sign(X), Math.Sign(Y));
    public Vector2Int Abs() => new Vector2Int(Math.Abs(X), Math.Abs(Y));
    public int Max() => Math.Max(X, Y);
    public int Min() => Math.Min(X, Y);
    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);
    public static bool operator ==(Vector2Int lhs, Vector2Int rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;
    public static bool operator !=(Vector2Int lhs, Vector2Int rhs) => !(lhs == rhs);
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Vector2Int v && Equals(v);
    public bool Equals(Vector2Int other) => X == other.X && Y == other.Y;
    public override int GetHashCode() => (X, Y).GetHashCode();
    public override string ToString() => $"({X}, {Y})";
    public static Vector2Int Zero => zero;
    public static Vector2Int Up => up;
    public static Vector2Int Down => down;
    public static Vector2Int Left => left;
    public static Vector2Int Right => right;
    private static readonly Vector2Int zero = new (0, 0);
    private static readonly Vector2Int up = new (0, 1);
    private static readonly Vector2Int down = new (0, -1);
    private static readonly Vector2Int left = new(-1, 0);
    private static readonly Vector2Int right = new(1, 0);
}
