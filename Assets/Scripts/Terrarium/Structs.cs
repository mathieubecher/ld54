using System;

[Serializable] public struct Position
{
    public int x;
    public int y;

    public Position(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
    public static Position operator +(Position a, Position b) => new Position(a.x + b.x, a.y + b.y);
    public static Position operator +(Position a) => a;
    public static Position operator -(Position a, Position b) => new Position(a.x - b.x, a.y - b.y);
    public static Position operator -(Position a) => new Position(- a.x, - a.y);
    public static bool operator ==(Position a, Position b) => a.x == b.x && a.y == b.y;
    public static bool operator !=(Position a, Position b) => a.x != b.x || a.y != b.y;
    public override string ToString() => $"[{x} : {y}]";
}

[Serializable] public struct Neighbours
{
    public Tile left;
    public Tile right;
    public Tile up;
    public Tile bottom;

    public Neighbours(Tile _left, Tile _right, Tile _up, Tile _bottom)
    {
        left = _left;
        right = _right;
        up = _up;
        bottom = _bottom;
    }
}