namespace oMethods_2;

public class BFGS : IMinMethodND {
    private Argument _min;
    public Argument Min => _min;
    public bool Need1DSearch => true;
    public int MaxIters { get; init; }
    public double Eps { get; init; }

    public BFGS(int maxIters, double eps) {
        MaxIters = maxIters;
        Eps = eps;
    }

    public void Compute(Argument initPoint, IFunction function, IMinMethod1D method) {
        int n = initPoint.Number;
        double lambda;
        int iters;
        double h = 1E-14;

        List<Argument> coords = new(); // для графики
        Matrix H = new(n);
        Matrix H1 = new(n);
        Argument nextPoint;
        Argument direction = new(n); // вектор, определяющий направление
        Argument deltaF = new(n);
        Argument deltaF1 = new(n); // производные по n переменным(в нашем случае 2D)
        Argument s; // $x_{k+1} - x_{k}$ - шаг алгоритма на итерации
        Argument y; // изменение градиента на итерации

        Argument denominatorAsVector;
        Argument numerator;
        double denominatorAsNumber;

        for (int i = 0; i < H.Size; i++) // начальный гессиан (единичная матрица)
            H[i, i] = 1;

        for (iters = 0; iters < MaxIters; iters++) {
            direction.Fill(0);
            coords.Add(initPoint);

            denominatorAsNumber = 0;

            if (iters % 2 == n && iters != 0) {
                H.Clear();

                for (int i = 0; i < H.Size; i++)
                    H[i, i] = 1;
            }

            for (int i = 0; i < n; i++)
                deltaF[i] = Derivative(initPoint, function, i, h);

            if (Norm(deltaF) < Eps) {
                _min = (Argument)initPoint.Clone();
                break;
            }

            direction = -H * deltaF;


            method.Compute(SearcherInterval.Search(function, 0, Eps, direction, initPoint),function, initPoint, direction);
            lambda = method.Min;
            nextPoint = (Argument)(initPoint + lambda * direction).Clone();

            for (int i = 0; i < n; i++)
                deltaF1[i] = Derivative(nextPoint, function, i, h);

            y = deltaF1 - deltaF;
            s = nextPoint - initPoint;

            if (s.Equals(lambda * direction)) {
                H.Clear();

                for (int i = 0; i < H.Size; i++)
                    H[i, i] = 1;

                initPoint = (Argument)nextPoint.Clone();
                continue;
            }

            denominatorAsVector = s - H * y; // вектор в знаменателе соотношения для deltaH

            for (int i = 0; i < n; i++)
                denominatorAsNumber += denominatorAsVector[i] * y[i]; // подсчитанный знаменатель

            numerator = (Argument)denominatorAsVector.Clone();

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    H1[i, j] = numerator[i] * numerator[j];

            if (Math.Abs(denominatorAsNumber) < Eps) {
                H.Clear();

                for (int i = 0; i < H.Size; i++)
                    H[i, i] = 1;

                initPoint = (Argument)nextPoint.Clone();
                continue;
            }

            H1 /= denominatorAsNumber;
            H += H1;
            initPoint = (Argument)nextPoint.Clone();
        }

        Console.WriteLine($"Iterations: {iters}");
        Console.WriteLine(JsonConvert.SerializeObject(_min));
        Console.WriteLine($"f(extremum) = {function.Value(_min)}");

        var sw = new StreamWriter("coords.txt");
        using(sw) {
            for (int i = 0; i < coords.Count; i++)
                sw.WriteLine(coords[i]);
        }
    }

    private static double Norm(Argument arg) {
        double result = 0;

        for (int i = 0; i < arg.Number; i++) {
            result += arg[i] * arg[i];
        }

        return Math.Sqrt(result);
    }

    private static double Derivative(Argument point, IFunction function, int numberVariable, double h) {
        Argument arg = new(point.Number);
        arg[numberVariable] = h;

        return (function.Value(point + arg) - function.Value(point - arg)) / (2 * h);
    }
}