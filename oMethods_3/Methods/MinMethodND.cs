namespace oMethods_3;

public interface IMinMethodND {
    public Argument? Min { get; }
    public int MaxIters { get; init; }
    public double Eps { get; init; }
    public bool Need1DSearch { get; }

    public void Compute(Argument initPoint, IFunction function, IMinMethod1D method, MethodTypes type, double coef);
}

public class GaussAlgorithm : IMinMethodND {
    private Argument? _min;
    public Argument? Min => _min;
    public int MaxIters { get; init; }
    public double Eps { get; init; }
    public bool Need1DSearch => true;

    public GaussAlgorithm(int maxIters, double eps) {
        MaxIters = maxIters;
        Eps = eps;
    }

    public void Compute(Argument initPoint, IFunction function, IMinMethod1D method, MethodTypes type, double coef) {
        List<Argument> coords = new();
        Argument direction = new(initPoint.Number);
        Argument nextPoint;
        int iters;

        nextPoint = (Argument)initPoint.Clone();
        coords.Add(initPoint);

        for (iters = 0; iters < MaxIters; iters++) {
            for (int i = 0; i < initPoint.Number; i++) {
                direction.Fill(0);
                direction[i] = 1;
                method.Compute(SearcherInterval.Search(function, 0, Eps, direction, nextPoint), function, nextPoint, direction);
                nextPoint[i] = initPoint[i] + method.Min!.Value;
            }

            coords.Add((Argument)nextPoint.Clone());

            if (Math.Abs(function.Value(nextPoint) - function.Value(initPoint)) < Eps || (nextPoint - initPoint).Norm() < Eps) {
                _min = (Argument)nextPoint.Clone();
                break;
            }

            initPoint = (Argument)nextPoint.Clone();
        }

        if (iters == MaxIters)
            _min = (Argument)nextPoint.Clone();

        Console.WriteLine($"Iterations: {iters}");
        Console.WriteLine(JsonConvert.SerializeObject(_min));

        var sw = new StreamWriter("coords.txt");
        using(sw) {
            for (int i = 0; i < coords.Count; i++)
                sw.WriteLine(coords[i]);
        }

        Console.WriteLine($"f(extremum) = {function.Value(_min!)}");
    }
}

public class SimplexMethod : IMinMethodND {
    private Argument? _min;
    public Argument? Min => _min;
    public bool Need1DSearch => false;
    public int MaxIters { get; init; }
    public double Step { get; init; }
    public double Eps { get; init; }

    public SimplexMethod(int maxIters, double step, double eps) {
        MaxIters = maxIters;
        Step = step;
        Eps = eps;
    }

    public void Compute(Argument initPoint, IFunction function, IMinMethod1D method, MethodTypes type, double coef) {
        List<Argument> coords = new();  // for graphics
        Argument[] points = new Argument[initPoint.Number + 1];

        int iters;
        int n = initPoint.Number;

        Argument xG = new(n);
        Argument xR = new(n);
        Argument xC = new(n);
        Argument xE = new(n);

        double d1 = Step * (Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2));
        double d2 = Step / ((n * Math.Sqrt(2)) * (Math.Sqrt(n + 1) - 1));

        points[2] = (Argument)initPoint.Clone();
        points[0] = new(n);
        points[1] = new(n);

        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++) {
                if (i == j) {
                    points[i][j] = d1;
                    continue;
                }

                points[i][j] = d2;
            }

        coords.Add(initPoint);

        for (iters = 0; iters < MaxIters; iters++) {
            points = points.OrderBy(point => function.Value(point)).ToArray();
            coords.Add(points[0]);
            xG.Fill(0);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    xG[i] += points[j][i] / n;

            if (Criteria(points, xG, function)) {
                _min = points[0];
                break;
            }

            Reflection(points, xG, xR);

            double value = function.Value(xR);

            if (function.Value(points[0]) <= value
                && value < function.Value(points[n - 1])) {
                points[n] = (Argument)xR.Clone();
            } else if (function.Value(xR) < function.Value(points[0])) {
                Expansion(xG, xR, xE);

                if (function.Value(xE) < value) {
                    points[n] = (Argument)xE.Clone();
                } else
                    points[n] = (Argument)xR.Clone();
            } else if (value < function.Value(points[n])) {
                OutsideContraction(xG, xR, xC);

                if (function.Value(xC) < value) {
                    points[n] = (Argument)xC.Clone();
                } else
                    Shrink(points);
            } else {
                InsideContraction(points, xG, xC);

                if (function.Value(xC) < function.Value(points[n])) {
                    points[n] = (Argument)xC.Clone();
                } else
                    Shrink(points);
            }
        }

        Console.WriteLine($"Iterations: {iters}");
        Console.WriteLine(JsonConvert.SerializeObject(_min = points[0]));

        using (var sw = new StreamWriter("coords.txt")) {
            for (int i = 0; i < coords.Count; i++)
                sw.WriteLine(coords[i]);
        }

        Console.WriteLine($"f(extremum) = {function.Value(_min)}");
    }

    private bool Criteria(Argument[] points, Argument xG, IFunction function) {
        double sum = 0;

        for (int i = 0; i < xG.Number + 1; i++) {
            sum += (function.Value(points[i]) - function.Value(xG)) *
                   (function.Value(points[i]) - function.Value(xG));
        }

        if (Math.Sqrt(sum / (xG.Number + 1)) < Eps)
            return true;
        else
            return false;
    }

    private static void Reflection(Argument[] points, Argument xG, Argument xR) {
        for (int i = 0; i < xG.Number; i++)
            xR[i] = xG[i] - points[xG.Number][i] + xG[i];
    }

    private static void Expansion(Argument xG, Argument xR, Argument xE) {
        for (int i = 0; i < xG.Number; i++)
            xE[i] = xG[i] + 2 * (xR[i] - xG[i]);
    }

    private static void OutsideContraction(Argument xG, Argument xR, Argument xC) {
        for (int i = 0; i < xG.Number; i++)
            xC[i] = xG[i] + 0.5 * (xR[i] - xG[i]);
    }

    private static void InsideContraction(Argument[] points, Argument xG, Argument xC) {
        for (int i = 0; i < xG.Number; i++)
            xC[i] = xG[i] + 0.5 * (points[xG.Number][i] - xG[i]);
    }

    private static void Shrink(Argument[] points) {
        for (int i = 1; i <= points[0].Number; i++)
            points[i] = (Argument)(points[0] + 0.5 * (points[i] - points[0])).Clone();
    }
}