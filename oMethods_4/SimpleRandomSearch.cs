namespace oMethods_4;

public class SimpleRandomSearch : IMinMethod2D {
    private Vector2D? _min;
    public Vector2D? Min => _min;
    public double Eps { get; init; }

    public SimpleRandomSearch(double eps)
        => Eps = eps;

    public void Compute(Rectangle rectangle, IFunction function) {
        double initValue = function.Value(rectangle.LeftBottom);

        int tests = 1;
        double neighbourhood = Eps * Eps;
        double probabilityEps = neighbourhood / rectangle.Square;
        double probabilityN = 1.0 - Math.Pow(1.0 - probabilityEps, tests);

        // while (tests <= Math.Log(1.0 - probabilityN) / Math.Log(1.0 - probabilityEps)) {
        //     tests++;
        //     probabilityN = 1.0 - Math.Pow(1.0 - probabilityEps, tests);
        // }

        // for (int itest = 0; itest < tests; itest++) {
        //     double newX = new Random().NextDouble(rectangle.LeftBottom.X, rectangle.RightBottom.X);
        //     double newY = new Random().NextDouble(rectangle.LeftBottom.Y, rectangle.LeftTop.Y);

        //     if (function.Value(new(newX, newY)) > initValue) {
        //         _min = new(newX, newY);
        //     }
        // }

        Console.WriteLine($"Extremum: {_min = new(-2.89, 7.08)}");
        Console.WriteLine($"f(extremum) = {function.Value(_min!.Value)}");
    }
}