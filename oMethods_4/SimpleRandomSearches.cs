namespace oMethods_4;

public class UndirectedSimpleRandomSearch : ISearchMethod2D {
    private Vector2D? _min;
    public Vector2D? Min => _min;
    public double Eps { get; init; }
    public double Probability { get; init; }

    public UndirectedSimpleRandomSearch(double eps, double probability)
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

            if ((temp = function.Value(new(newX, newY))) < functionMaxValue) {
                _min = new(newX, newY);
                functionMaxValue = temp;
            }
        }

        Console.WriteLine($"N: {tests}");
        Console.WriteLine($"P: {Probability}");
        Console.WriteLine($"Extremum: {_min}");
        Console.WriteLine($"f(extremum) = {-functionMaxValue}");
    }
}

public class SimplexMethod : IMinMethod2D {
    private Vector2D? _min;
    public Vector2D? Min => _min;
    public int MaxIters { get; init; }
    public double Step { get; init; }
    public double Eps { get; init; }

    public SimplexMethod(int maxIters, double step, double eps) {
        MaxIters = maxIters;
        Step = step;
        Eps = eps;
    }

    public void Compute(IFunction function, Vector2D initPoint) {
        Vector2D[] points = new Vector2D[3];

        double d1 = Step * (Math.Sqrt(2 + 1) + 2 - 1) / (2 * Math.Sqrt(2));
        double d2 = Step / (2 * Math.Sqrt(2) * (Math.Sqrt(2 + 1) - 1));

        points[0] = initPoint;
        points[1] = new(d1, d2);
        points[2] = new(d2, d1);

        for (int iter = 0; iter < MaxIters; iter++) {
            Vector2D xG;
            Vector2D xR;
            Vector2D xC;
            Vector2D xE;

            double sumX = 0;
            double sumY = 0;

            points = points.OrderBy(point => function.Value(point)).ToArray();

            for (int i = 0; i < 2; i++) {
                sumX += points[i].X;
                sumY += points[i].Y;
            }

            xG = new(sumX / 2.0, sumY / 2.0);

            if (Criteria(points, xG, function)) {
                _min = points[0];
                break;
            }

            xR = Reflection(points, xG);

            double value = function.Value(xR);

            if (function.Value(points[0]) <= value
                && value < function.Value(points[1])) {
                points[2] = xR;
            } else if (value < function.Value(points[0])) {
                xE = Expansion(xG, xR);

                if (function.Value(xE) < value) {
                    points[2] = xE;
                } else {
                    points[2] = xR;
                }
            } else if (value < function.Value(points[2])) {
                xC = OutsideContraction(xG, xR);

                if (function.Value(xC) < value) {
                    points[2] = xC;
                } else {
                    Shrink(points);
                }
            } else {
                xC = InsideContraction(points, xG);

                if (function.Value(xC) < function.Value(points[2])) {
                    points[2] = xC;
                } else {
                    Shrink(points);
                }
            }
        }

        _min = points[0];
    }

    private bool Criteria(Vector2D[] points, Vector2D xG, IFunction function) {
        double sum = 0.0;
        double valueXG = function.Value(xG);

        for (int i = 0; i < 3; i++) {
            double valuePoint = function.Value(points[i]);

            sum += (valuePoint - valueXG) * (valuePoint - valueXG);
        }

        return Math.Sqrt(sum / (2 + 1)) < Eps;
    }

    private static Vector2D Reflection(Vector2D[] points, Vector2D xG)
        => new(xG.X - points[2].X + xG.X, xG.Y - points[2].Y + xG.Y);

    private static Vector2D Expansion(Vector2D xG, Vector2D xR)
        => new(xG.X + (2 * (xR.X - xG.X)), xG.Y + (2 * (xR.Y - xG.Y)));

    private static Vector2D OutsideContraction(Vector2D xG, Vector2D xR)
        => new(xG.X + (0.5 * (xR.X - xG.X)), xG.Y + (0.5 * (xR.Y - xG.Y)));

    private static Vector2D InsideContraction(Vector2D[] points, Vector2D xG)
         => new(xG.X + (0.5 * (points[2].X - xG.X)), xG.Y + (0.5 * (points[2].Y - xG.Y)));

    private static void Shrink(Vector2D[] points) {
        for (int i = 1; i <= 2; i++)
            points[i] = points[0] + (0.5 * (points[i] - points[0]));
    }
}

// public class DirectedSimpleRandomSearch : ISearchMethod2D { //
//     private Vector2D? _min;
//     public Vector2D? Min => _min;
//     public double Eps { get; init; }
//     public double Probability { get; init; }

//     public DirectedSimpleRandomSearch(double eps, double probability)
//         => (Eps, Probability) = (eps, probability);

//     public void Compute(Rectangle rectangle, IFunction function) {
//         double functionMaxValue = function.Value(rectangle.LeftBottom);

//         int tests = 1;
//         const double step = 1.0;
//         double neighbourhood = Eps * Eps;
//         double probabilityEps = neighbourhood / rectangle.Square;

//         while (tests < Math.Log(1.0 - Probability) / Math.Log(1.0 - probabilityEps)) {
//             tests++;
//         }

//         for (int i = 0; i < tests; i++) {
//             double newX = new Random().NextDouble(rectangle.LeftBottom.X, rectangle.RightBottom.X);
//             double newY = new Random().NextDouble(rectangle.LeftBottom.Y, rectangle.LeftTop.Y);

//             double temp;

//             if ((temp = function.Value(new(newX, newY))) > functionMaxValue) {
//                 _min = new(newX, newY);
//                 functionMaxValue = temp;

//                 Vector2D unitVector = new(1.0 / (rectangle.RightBottom.X - rectangle.LeftBottom.X),
//                                                    1.0 / (rectangle.LeftTop.Y - rectangle.LeftBottom.Y));

//                 unitVector = unitVector.Normalize();

//                 if (function.Value(_min.Value + unitVector) > function.Value(_min.Value - unitVector)) {
//                     _min = new(_min.Value.X + (step * unitVector.X), _min.Value.Y + (step * unitVector.Y));
//                 } else {
//                     _min = new(_min.Value.X - (step * unitVector.X), _min.Value.Y - (step * unitVector.Y));
//                 }
//             }
//         }
//     }
// }