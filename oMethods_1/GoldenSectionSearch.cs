namespace oMethods_1;

public class GoldenSectionSearch : IMinMethod
{
    private double _min;
    public double Min
        => _min;

    public void Compute(Interval interval, IFunction function, double eps, double sigma)
    {
        double x1 = interval.A + ((3 - Math.Sqrt(5)) / 2) * (interval.B - interval.A);
        double x2 = interval.A + ((Math.Sqrt(5) - 1) / 2) * (interval.B - interval.A);
        double y1 = function.Func(x1);
        double y2 = function.Func(x2);

        while (interval.B - interval.A > eps)
        {
            if (y1 < y2)
            {
                interval = new(interval.A, x2);
                x2 = x1;
                y2 = y1;
                x1 = interval.A + (interval.B - x2);
                y1 = function.Func(x1);
            }
            else
            {
                interval = new(x1, interval.B);
                x1 = x2;
                y1 = y2;
                x2 = interval.B - (x1 - interval.A);
                y2 = function.Func(x2);
            }
        }

        _min = interval.Center;
    }
}