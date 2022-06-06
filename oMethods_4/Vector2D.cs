namespace oMethods_4;

public readonly record struct Vector2D(double X, double Y) {
    [JsonIgnore]
    public double Norm => Math.Sqrt((X * X) + (Y * Y));

    public static Vector2D operator +(Vector2D a, Vector2D b)
    => new(a.X + b.X, a.Y + b.Y);

    public static Vector2D operator -(Vector2D a, Vector2D b)
        => new(a.X - b.X, a.Y - b.Y);

    public static Vector2D operator /(Vector2D a, double v)
        => new(a.X / v, a.Y / v);

    public static Vector2D operator *(Vector2D a, double v)
        => new(a.X * v, a.Y * v);

    public static Vector2D operator *(double v, Vector2D a)
        => new(v * a.X, v * a.Y);

    public static double Distance(Vector2D a, Vector2D b)
        => (a - b).Norm;

    public double Distance(Vector2D vector)
        => Distance(this, vector);

    public Vector2D Normalize() => this / Norm;

    public static Vector2D Zero
        => new(0.0, 0.0);

    public override string ToString()
        => $"{X} {Y}";
}