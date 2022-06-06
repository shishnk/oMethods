namespace oMethods_4;

public class AlgorithmA : ISearchMethod2D {
    private Vector2D? _min;
    public Vector2D? Min => _min;
    public double Eps { get; init; }
    public int Trying { get; init; }

    public AlgorithmA(double eps, int trying)
        => (Eps, Trying) = (eps, trying);

    public void Compute(Rectangle rectangle, IFunction function) {
        IMinMethod2D method = new SimplexMethod(1000, 1.0, Eps);
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

public class AlgorithmB : ISearchMethod2D {
    private Vector2D? _min;
    public Vector2D? Min => _min;
    public double Eps { get; init; }
    public int Trying { get; init; }

    public AlgorithmB(double eps, int trying)
        => (Eps, Trying) = (eps, trying);

    public void Compute(Rectangle rectangle, IFunction function) {
        IMinMethod2D simplexMethod = new SimplexMethod(1000, 1.0, Eps);
        Vector2D point;

        double newX = new Random().NextDouble(rectangle.LeftBottom.X, rectangle.RightBottom.X);
        double newY = new Random().NextDouble(rectangle.LeftBottom.Y, rectangle.LeftTop.Y);

        double temp;

        simplexMethod.Compute(function, new(newX, newY));
        _min = simplexMethod.Min;

        double functionValue = function.Value(_min!.Value);

        for (int i = 0; i < Trying; i++) {
            do {
                newX = new Random().NextDouble(rectangle.LeftBottom.X, rectangle.RightBottom.X);
                newY = new Random().NextDouble(rectangle.LeftBottom.Y, rectangle.LeftTop.Y);
                point = new(newX, newY);
            } while ((temp = function.Value(point)) >= functionValue);

            _min = point;
            functionValue = temp;

            newX = new Random().NextDouble(rectangle.LeftBottom.X, rectangle.RightBottom.X);
            newY = new Random().NextDouble(rectangle.LeftBottom.Y, rectangle.LeftTop.Y);

            simplexMethod.Compute(function, new(newX, newY));

            if ((temp = function.Value(simplexMethod.Min!.Value)) < functionValue) {
                _min = simplexMethod.Min;
                functionValue = temp;
            }
        }

        Console.WriteLine($"Extremum: {_min}");
        Console.WriteLine($"f(extremum) = {-function.Value(_min!.Value)}");
    }
}

public class AlgorithmC : ISearchMethod2D {
    private Vector2D? _min;
    public Vector2D? Min => _min;
    public double Eps { get; init; }
    public int Trying { get; init; }

    public AlgorithmC(double eps, int trying)
        => (Eps, Trying) = (eps, trying);

    public void Compute(Rectangle rectangle, IFunction function) {
        IMinMethod2D simplexMethod = new SimplexMethod(1000, 1.0, Eps);

        double newX = new Random().NextDouble(rectangle.LeftBottom.X, rectangle.RightBottom.X);
        double newY = new Random().NextDouble(rectangle.LeftBottom.Y, rectangle.LeftTop.Y);

        double temp;
        double functionValueX2;

        Vector2D x1, x2;
        Vector2D initPoint = new(newX, newY);
        _min = new(newX, newY);

        double functionValueX1 = function.Value(_min.Value);

        for (int i = 0; i < Trying; i++) {
            double step = 1.0;

            simplexMethod.Compute(function, initPoint);
            x1 = simplexMethod.Min!.Value;

            if ((temp = function.Value(simplexMethod.Min!.Value)) < functionValueX1) {
                _min = simplexMethod.Min;
                functionValueX1 = temp;
            }

            newX = new Random().NextDouble(rectangle.LeftBottom.X, rectangle.RightBottom.X);
            newY = new Random().NextDouble(rectangle.LeftBottom.Y, rectangle.LeftTop.Y);

            Vector2D randomVector = new(newX, newY);
            Vector2D direction = simplexMethod.Min!.Value + (step * (simplexMethod.Min.Value - randomVector));
            double functionValueDirection = function.Value(direction);

            while (functionValueDirection >= function.Value(simplexMethod.Min.Value) && rectangle.Inside(direction)) {
                direction = simplexMethod.Min!.Value + (step * (simplexMethod.Min.Value - randomVector));
                functionValueDirection = function.Value(direction);
                step++;
            }

            simplexMethod.Compute(function, direction);
            functionValueX2 = function.Value(simplexMethod.Min.Value);

            x2 = simplexMethod.Min.Value;

            if (functionValueX2 < functionValueX1) {
                _min = simplexMethod.Min;
            }

            if (functionValueX2 < functionValueX1) {
                initPoint = x2;
            } else {
                initPoint = x1;
            }
        }

        Console.WriteLine($"Extremum: {_min}");
        Console.WriteLine($"f(extremum) = {-function.Value(_min!.Value)}");
    }
}