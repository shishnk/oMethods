namespace oMethods_2;

public class Polynom : IFunction {
    public double Value(Point2D point)
        => (point.X * point.X + point.Y * point.Y) *
            (point.X * point.X + point.Y * point.Y) -
            (point.X - 3 * point.Y) * (point.X - 3 * point.Y);
}