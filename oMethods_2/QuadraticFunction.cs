namespace oMethods_2;

public class QuadraticFunction : IFunction {
    public double Value(Point2D point)
        => 100 * (point.Y - point.X) * (point.Y - point.X) +
        (1 - point.X) * (1 - point.X);
}