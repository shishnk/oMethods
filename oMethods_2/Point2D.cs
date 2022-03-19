namespace oMethods_2;

public readonly record struct Point2D(double X, double Y) {
    public override string ToString()
        => $"{X} {Y}";

    public static Point2D operator +(Point2D point, (double, double) value)
       => new(point.X + value.Item1, point.Y + value.Item2);

    public static Point2D operator -(Point2D point, (double, double) value)
       => new(point.X - value.Item1, point.Y - value.Item2);

    public static Point2D operator +(Point2D fstPoint, Point2D sndPoint)
        => new(fstPoint.X + sndPoint.X, fstPoint.Y + sndPoint.Y);

    public static Point2D operator -(Point2D fstPoint, Point2D sndPoint)
       => new(fstPoint.X - sndPoint.X, fstPoint.Y - sndPoint.Y);

    public static Point2D operator *(double constant, Point2D point)
       => new(constant * point.X, constant * point.Y);
}