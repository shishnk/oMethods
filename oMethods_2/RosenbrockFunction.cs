namespace oMethods_2;

public class RosenbrockFunction : IFunction {
    public double Value(Point2D point)
        => 100 * (point.Y - point.X * point.X) * (point.Y - point.X * point.X) +
        (1 - point.X) * (1 - point.X);
}