namespace oMethods_2;

public class Function : IFunction { // 8 variant(Maximum)
    public double Value(Point2D point)
        => -(3.0 / (1 + (point.X - 2) * (point.X - 2) +
        ((point.Y - 2) / 2) * (point.Y - 2) / 2) +
        2.0 / (1 + ((point.X - 2) / 3) * ((point.X - 2) / 3) +
        (point.Y - 3) * (point.Y - 3)));
}