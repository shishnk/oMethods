namespace oMethods_2;

public class QuadraticInterpolation : IMinMethod1D {
    private double _min;
    public double Min => _min;
    public double Eps { get; init; }

    public QuadraticInterpolation(double eps)
        => Eps = eps;

    public void Compute(Interval interval, IFunction function, Argument argument, Argument direction) {
        double xk, x1, x2, b, c;
        int iters;
        double x0 = interval.Center;
        double step = (interval.Right - interval.Left) / 2;

        for (iters = 0; ; iters++) {
            x1 = x0 - step;
            x2 = x0 + step;

            c = (function.Value(argument + x1 * direction) -
                2 * function.Value(argument + x0 * direction) +
                function.Value(argument + x2 * direction)) /
                (2 * step * step);

            b = (-function.Value(argument + x1 * direction) *
                (2 * x0 + step) + 4 * function.Value(argument + x0 * direction) *
                x0 - function.Value(argument + x2 * direction) *
                (2 * x0 - step)) / (2 * step * step);

            xk = -b / (2 * c);

            if (Math.Abs(xk - x0) < Eps) {
                _min = xk;
                break;
            } else
                x0 = xk;
        }

        // Console.WriteLine(iters);
    }
}