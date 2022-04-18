namespace oMethods_3;

public interface IMinMethod1D {
    public double? Min { get; }
    public double Eps { get; init; }

    // fore report
    public int Call { get; set; }
    //

    public void Compute(Interval interval, IFunction function, Argument argument, Argument direction);
}

public class Fibonacci : IMinMethod1D {
    private double? _min;
    public double? Min => _min;
    public double Eps { get; init; }

    // for report
    public int Call { get; set; }
    //

    public Fibonacci(double eps)
        => (Eps, Call) = (eps, 0);

    public void Compute(Interval interval, IFunction function, Argument argument, Argument direction) {
        int n = 1;

        while ((interval.Right - interval.Left) / Eps > Fib(n + 2))
            n++;

        double x1 = interval.Left + Fib(n) / Fib(n + 2) * (interval.Right - interval.Left);
        double x2 = interval.Left + Fib(n + 1) / Fib(n + 2) * (interval.Right - interval.Left);
        double y1 = function.Value(argument + x1 * direction) + function.PenaltyValue(argument + x1 * direction);
        double y2 = function.Value(argument + x2 * direction) + function.PenaltyValue(argument + x2 * direction);

        Call += 2;

        for (int k = 1; k < n; k++) {
            if (y1 < y2) {
                interval = new(interval.Left, x2);
                x2 = x1;
                y2 = y1;
                x1 = interval.Left + Fib(n - k + 1) / Fib(n - k + 3) * (interval.Right - interval.Left);
                y1 = function.Value(argument + x1 * direction) + function.PenaltyValue(argument + x1 * direction);
                Call++;
            } else {
                interval = new(x1, interval.Right);
                x1 = x2;
                y1 = y2;
                x2 = interval.Left + Fib(n - k + 2) / Fib(n - k + 3) * (interval.Right - interval.Left);
                y2 = function.Value(argument + x2 * direction) + function.PenaltyValue(argument + x2 * direction);
                Call++;
            }
        }

        _min = interval.Center;
    }

    private static double Fib(int n)
        => (Math.Pow((1 + Math.Sqrt(5)) / 2, n) -
            Math.Pow((1 - Math.Sqrt(5)) / 2, n)) / Math.Sqrt(5);
}