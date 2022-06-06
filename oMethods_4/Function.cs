namespace oMethods_4;

public interface IFunction {
    public double Value(Vector2D point);
}

public class Function : IFunction {
    public double Value(Vector2D point)
        => -((2.0 / (1 + ((point.X - 5) * (point.X - 5)) + ((point.Y - 4) * (point.Y - 4)))) +
           (1.0 / (1 + ((point.X - 2) * (point.X - 2)) + (point.Y * point.Y))) +
           (7.0 / (1 + ((point.X + 9) * (point.X + 9)) + ((point.Y + 6) * (point.Y + 6)))) +
           (2.0 / (1 + (point.X * point.X) + ((point.Y + 3) * (point.Y + 3)))) +
           (8.0 / (1 + ((point.X + 3) * (point.X + 3)) + ((point.Y - 7) * (point.Y - 7)))) +
           (4.0 / (1 + ((point.X + 3) * (point.X + 3)) + ((point.Y - 3) * (point.Y - 3)))));
}