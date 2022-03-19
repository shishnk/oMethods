namespace oMethods_2;

public class ExpFunction : IFunction {
    public double Value(Point2D point) // Maximum
        => -(3 * Math.Exp(-(point.X - 2) * (point.X - 2) - ((point.Y - 3) / 2) * ((point.Y - 3) / 2)) +
        Math.Exp(-((point.X - 1) / 2) * ((point.X - 1) / 2) - (point.Y - 1) * (point.Y - 1)));
}