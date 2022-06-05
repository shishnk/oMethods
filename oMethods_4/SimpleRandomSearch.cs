namespace oMethods_4;

public class SimpleRandomSearch : IMinMethod2D {
    private Vector2D? _min;
    public Vector2D? Min => _min;
    public double Eps { get; init; }
    public double Probability { get; init; }

    public SimpleRandomSearch(double eps, double probability)
        => (Eps, Probability) = (eps, probability);

    public void Compute(Rectangle rectangle, IFunction function) {
        double functionMaxValue = function.Value(rectangle.LeftBottom);

        int tests = 1;
        double neighbourhood = Eps * Eps;
        double probabilityEps = neighbourhood / rectangle.Square;

        while (tests < Math.Log(1.0 - Probability) / Math.Log(1.0 - probabilityEps)) {
            tests++;
        }

        for (int itest = 0; itest < tests; itest++) {
            double newX = new Random().NextDouble(rectangle.LeftBottom.X, rectangle.RightBottom.X);
            double newY = new Random().NextDouble(rectangle.LeftBottom.Y, rectangle.LeftTop.Y);

            double temp;

            if ((temp = function.Value(new(newX, newY))) > functionMaxValue) {
                _min = new(newX, newY);
                functionMaxValue = temp;
            }
        }

        Console.WriteLine($"N: {tests}");
        Console.WriteLine($"P: {Probability}");
        Console.WriteLine($"Extremum: {_min}");
        Console.WriteLine($"f(extremum) = {functionMaxValue}");
    }
}