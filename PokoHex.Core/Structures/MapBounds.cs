namespace PokoHex.Core;

public readonly record struct MapBounds(int X, int Y, int Z, int Width, int Height, int Depth)
{
    public bool Contains(int x, int y, int z) => x >= X && x < X + Width &&
        y >= Y && y < Y + Height && z >= Z && z < Z + Depth;

    public bool Intersects(MapBounds other) => X < other.X + other.Width && X + Width > other.X &&
        Y < other.Y + other.Height && Y + Height > other.Y && Z < other.Z + other.Depth && Z + Depth > other.Z;
}
