namespace oMethods_2;

public class SimplexMethod : IMinMethodND {
    private Point2D _min;
    public Point2D Min
        => _min;
    public int MaxIters { get; init; }
    public double Step { get; init; }
    public double Eps { get; init; }

    public SimplexMethod(int maxIters, double step, double eps) {
        MaxIters = maxIters;
        Step = step;
        Eps = eps;
    }

    public void Compute(Point2D initPoint, IFunction function) {
        List<Point2D> coords = new();  // for graphics
        Point2D[] points = new Point2D[3];
        int iters;

        double d1 = Step * (Math.Sqrt(2 + 1) + 2 - 1) / (2 * Math.Sqrt(2));
        double d2 = Step / ((2 * Math.Sqrt(2)) * (Math.Sqrt(2 + 1) - 1));

        points[0] = initPoint;
        points[1] = new(d1, d2);
        points[2] = new(d2, d1);
        coords.Add(initPoint);

        for (iters = 0; iters < MaxIters; iters++) {
            Point2D xG;
            Point2D xR;
            Point2D xC;
            Point2D xE;

            double sumX = 0;
            double sumY = 0;

            points = points.OrderBy(point => function.Value(point)).ToArray();
            coords.Add(points[0]);

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

            if (function.Value(points[0]) <= function.Value(xR)
                && function.Value(xR) < function.Value(points[1])) {
                points[2] = xR;
                continue;
            } else if (function.Value(xR) < function.Value(points[0])) {
                xE = Expansion(xG, xR);

                if (function.Value(xE) < function.Value(xR)) {
                    points[2] = xE;
                    continue;
                } else {
                    points[2] = xR;
                    continue;
                }
            } else if (function.Value(xR) < function.Value(points[2])) {
                xC = OutsideContraction(xG, xR);

                if (function.Value(xC) < function.Value(xR)) {
                    points[2] = xC;
                    continue;
                } else {
                    Shrink(points);
                    continue;
                }
            } else {
                xC = InsideContraction(points, xG);

                if (function.Value(xC) < function.Value(points[2])) {
                    points[2] = xC;
                    continue;
                } else {
                    Shrink(points);
                    continue;
                }
            }
        }

        Console.WriteLine($"Iterations: {iters}");
        Console.WriteLine(JsonConvert.SerializeObject(_min = points[0]));

        using (var sw = new StreamWriter("coords.txt")) {
            for (int i = 0; i < coords.Count; i++)
                sw.WriteLine(coords[i]);
        }

        Console.WriteLine($"f({_min.X}; {_min.Y}) = {function.Value(_min)}");
    }

    private bool Criteria(Point2D[] points, Point2D xG, IFunction function) {
        double sum = 0;

        for (int i = 0; i < 3; i++) {
            sum += (function.Value(points[i]) - function.Value(xG)) *
                   (function.Value(points[i]) - function.Value(xG));
        }

        if (Math.Sqrt(sum / (2 + 1)) < Eps)
            return true;
        else
            return false;
    }

    private Point2D Reflection(Point2D[] points, Point2D xG)
        => new(xG.X - points[2].X + xG.X, xG.Y - points[2].Y + xG.Y);

    private Point2D Expansion(Point2D xG, Point2D xR)
        => new(xG.X + 2 * (xR.X - xG.X), xG.Y + 2 * (xR.Y - xG.Y));

    private Point2D OutsideContraction(Point2D xG, Point2D xR)
        => new(xG.X + 0.5 * (xR.X - xG.X), xG.Y + 0.5 * (xR.Y - xG.Y));

    private Point2D InsideContraction(Point2D[] points, Point2D xG)
         => new(xG.X + 0.5 * (points[2].X - xG.X), xG.Y + 0.5 * (points[2].Y - xG.Y));

    private void Shrink(Point2D[] points) {
        for (int i = 1; i <= 2; i++)
            points[i] = points[0] + 0.5 * (points[i] - points[0]);
    }
}