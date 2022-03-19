namespace oMethods_2;

public class BFGS : IMinMethodND {
    private Point2D _min;
    public Point2D Min
        => _min;
    public int MaxIters { get; init; }
    public double Eps { get; init; }

    public BFGS(int maxIters, double eps) {
        MaxIters = maxIters;
        Eps = eps;
    }

    public void Compute(Point2D initPoint, IFunction function) {
        Matrix H = new(2);
        Fibonacci fB = new(Eps);
        QuadraticInterpolation qI = new(Eps);
        double[] sum = new double[2];
        double h = 1E-14;
        double lambda;
        int iters;
        Point2D direction; // вектор, определяющий направление
        Point2D nextPoint = new(0, 0);
        Point2D deltaF;
        Point2D deltaF1; // производные по n переменным(в нашем случае 2D)
        Point2D s; // $x_{k+1} - x_{k}$ - шаг алгоритма на итерации
        Point2D y; // изменение градиента на итерации

        for (int i = 0; i < H.Size; i++) // начальный гессиан (единичная матрица)
            H[i, i] = 1;

        for (iters = 0; iters < MaxIters; iters++) {
            if (iters % 2 == 0 && iters != 0) {
                H.Clear();

                for (int i = 0; i < H.Size; i++)
                    H[i, i] = 1;
            }

            deltaF = new(Derivative(initPoint, function, 0, h), Derivative(initPoint, function, 1, h));

            if (Norm(deltaF) < Eps) {
                _min = initPoint;
                break;
            }

            for (int i = 0; i < H.Size; i++)
                for (int j = 0; j < H.Size; j++) {
                    sum[i] += -H[i, j] * Derivative(initPoint, function, j, h);
                }

            direction = new(sum[0], sum[1]);
            Array.Clear(sum);

            qI.Compute(SearcherInterval.Search(function, 0, Eps, direction, initPoint), function, initPoint, direction);
            lambda = qI.Min;
            nextPoint = initPoint + lambda * direction;
            deltaF1 = new(Derivative(nextPoint, function, 0, h), Derivative(nextPoint, function, 1, h));

            y = deltaF1 - deltaF;
            s = nextPoint - initPoint;

            if (s == lambda * direction) {
                H.Clear();

                for (int i = 0; i < H.Size; i++)
                    H[i, i] = 1;

                initPoint = nextPoint;
                continue;
            }


            double[] yArray = new double[2] { y.X, y.Y };

            Matrix H1 = new(H.Size);

            Point2D numerator = s - H * yArray;
            H1[0, 0] = numerator.X * numerator.X;
            H1[0, 1] = numerator.X * numerator.Y;
            H1[1, 0] = numerator.Y * numerator.X;
            H1[1, 1] = numerator.Y * numerator.Y;
            Point2D denominator = s - H * yArray;
            double denominator1 = denominator.X * y.X + denominator.Y * y.Y;

            if (Math.Abs(denominator1) < Eps) {
                H.Clear();

                for (int i = 0; i < H.Size; i++)
                    H[i, i] = 1;

                initPoint = nextPoint;
                continue;
            }

            H1 /= denominator1;
            H += H1;
            initPoint = nextPoint;
        }

        Console.WriteLine($"Iterations: {iters}");
        Console.WriteLine(JsonConvert.SerializeObject(_min));
        Console.WriteLine($"f({_min.X}; {_min.Y}) = {function.Value(_min)}");
    }
    private double Norm(Point2D point)
        => Math.Sqrt(point.X * point.X + point.Y * point.Y);

    private double Derivative(Point2D point, IFunction function, int numberVariable, double h)
        => (numberVariable == 0) ? (function.Value(point + (h, 0)) - function.Value(point - (h, 0))) / (2 * h) :
           (function.Value(point + (0, h)) - function.Value(point - (0, h))) / (2 * h);
}