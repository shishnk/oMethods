namespace oMethods_4;

public class AlgorithmA : ISearchMethod2D {
    private Vector2D? _min;
    public Vector2D? Min => _min;
    public double Eps { get; init; }
    public int Trying { get; init; }

    public AlgorithmA(double eps, int trying)
        => (Eps, Trying) = (eps, trying);

    public void Compute(Rectangle rectangle, IFunction function) {
        SimplexMethod method = new(1000, 1.0, Eps);
        _min = new(0.0, 0.0);

        for (int i = 0; i < Trying; i++) {
            double newX = new Random().NextDouble(rectangle.LeftBottom.X, rectangle.RightBottom.X);
            double newY = new Random().NextDouble(rectangle.LeftBottom.Y, rectangle.LeftTop.Y);

            method.Compute(function, new(newX, newY));

            if (function.Value(method.Min!.Value) < function.Value(_min.Value)) {
                _min = method.Min;
            }
        }

        Console.WriteLine($"Extremum: {_min}");
        Console.WriteLine($"f(extremum) = {-function.Value(_min!.Value)}");
    }
}

public class AlgorithmB {
}

public class AlgorithmC {
}