namespace oMethods_3;

public interface IMinMethodND {
    public Argument? Min { get; }
    public int MaxIters { get; init; }
    public double Eps { get; init; }
    public bool Need1DSearch { get; }

    public void Compute(Argument initPoint, IFunction function, IMinMethod1D method,
                        MethodTypes methodType, StrategyTypes strategyType);
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

    public void Compute(Argument initPoint, IFunction function, IMinMethod1D method,
                        MethodTypes methodType, StrategyTypes strategyType) {
        Argument direction = new(initPoint.Number);
        Argument nextPoint;
        int iters;

        nextPoint = (Argument)initPoint.Clone();

        for (iters = 0; iters < MaxIters; iters++) {
            for (int i = 0; i < initPoint.Number; i++) {
                direction.Fill(0);
                direction[i] = 1;
                method.Compute(SearcherInterval.Search(function, 0, Eps, direction, nextPoint),
                               function, nextPoint, direction);
                nextPoint[i] = initPoint[i] + method.Min!.Value;
            }

            if (function.PenaltyValue(nextPoint) < Eps &&
                function.Value(nextPoint) + function.PenaltyValue(nextPoint) -
                (function.Value(initPoint) + function.PenaltyValue(initPoint)) < Eps) {

                _min = (Argument)nextPoint.Clone();
                break;
            }

            initPoint = (Argument)nextPoint.Clone();

            function.Coef = strategyType switch {
                StrategyTypes.Multiply => function.Coef *= 2,
                StrategyTypes.Increment => function.Coef++,

                _ => throw new ArgumentOutOfRangeException(nameof(strategyType),
                    $"This type of coefficient change strategy does not exist: {strategyType}")
            };
        }

        if (iters == MaxIters)
            _min = (Argument)nextPoint.Clone();

        Console.WriteLine($"Iterations: {iters}");
        Console.WriteLine(JsonConvert.SerializeObject(_min));
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

    public void Compute(Argument initPoint, IFunction function, IMinMethod1D method,
                        MethodTypes methodType, StrategyTypes strategyType) {

        Argument[] points = new Argument[initPoint.Number + 1];

        int iters;
        int n = initPoint.Number;

        Argument xG = new(n);
        Argument xR = new(n);
        Argument xC = new(n);
        Argument xE = new(n);

        double d1 = Step * (Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2));
        double d2 = Step / (n * Math.Sqrt(2) * (Math.Sqrt(n + 1) - 1));

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

        for (iters = 0; iters < MaxIters; iters++) {
            points = points.OrderBy(point => function.Value(point) + function.PenaltyValue(point)).ToArray();
            xG.Fill(0);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    xG[i] += points[j][i] / n;

            if (Criteria(points, xG, function) && function.PenaltyValue(points[0]) < Eps) {
                _min = points[0];
                break;
            }

            if (iters != 0) {

                function.Coef = strategyType switch {
                    StrategyTypes.Multiply => function.Coef *= 2,
                    StrategyTypes.Increment => function.Coef++,

                    _ => throw new ArgumentOutOfRangeException(nameof(strategyType),
                        $"This type of coefficient change strategy does not exist: {strategyType}")
                };
            }

            Reflection(points, xG, xR);

            double valueXR = function.Value(xR) + function.PenaltyValue(xR);
            double valueBestPoint = function.Value(points[0]) + function.PenaltyValue(points[0]);
            double valueN = function.Value(points[n]) + function.PenaltyValue(points[n]);

            if (valueBestPoint <= valueXR
                && valueXR < function.Value(points[n - 1]) + function.PenaltyValue(points[n - 1])) {
                points[n] = (Argument)xR.Clone();
            } else if (valueXR < valueBestPoint) {
                Expansion(xG, xR, xE);

                if (function.Value(xE) + function.PenaltyValue(xE) < valueXR) {
                    points[n] = (Argument)xE.Clone();
                } else
                    points[n] = (Argument)xR.Clone();
            } else if (valueXR < valueN) {
                OutsideContraction(xG, xR, xC);

                if (function.Value(xC) + function.PenaltyValue(xC) < valueXR) {
                    points[n] = (Argument)xC.Clone();
                } else
                    Shrink(points);
            } else {
                InsideContraction(points, xG, xC);

                if (function.Value(xC) + function.PenaltyValue(xC) < valueN) {
                    points[n] = (Argument)xC.Clone();
                } else
                    Shrink(points);
            }
        }

        Console.WriteLine($"Iterations: {iters}");
        Console.WriteLine(JsonConvert.SerializeObject(_min = points[0]));
        Console.WriteLine($"f(extremum) = {function.Value(_min)}");
    }

    private bool Criteria(Argument[] points, Argument xG, IFunction function) {
        double sum = 0;

        for (int i = 0; i < xG.Number + 1; i++) {
            sum += (function.Value(points[i]) +
                    function.PenaltyValue(points[i]) - (function.Value(xG) + function.PenaltyValue(xG))) *
                   (function.Value(points[i]) + function.PenaltyValue(points[i]) -
                   (function.Value(xG) + function.PenaltyValue(xG)));
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