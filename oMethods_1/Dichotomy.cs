namespace oMethods_1;

public class Dichotomy : IMinMethod
{
    private double _min;
    public double Min
        => _min;

    public void Compute(Interval interval, IFunction function, double eps, double sigma)
    {
        while (interval.B - interval.A > eps)
        {
            double x1 = (interval.Center - sigma / 2);
            double x2 = (interval.Center + sigma / 2);

            if (function.Func(x1) < function.Func(x2))
                interval = new(interval.A, x1);
            else if (function.Func(x1) > function.Func(x2))
                interval = new(x2, interval.B);
            else
                interval = new(x1, x2);
        }

        _min = interval.Center;
    }
}