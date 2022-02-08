namespace oMethods_1;

public class Fibonacci : IMinMethod
{
    private double _min;
    public double Min
        => _min;

    public void Compute(Interval interval, IFunction function, double eps, double sigma)
    {
        int n = 1;

        while ((interval.B - interval.A) / eps > Fib(n + 2))
            n++;

        double x1 = interval.A + Fib(n) / Fib(n + 2) * (interval.B - interval.A);
        double x2 = interval.A + Fib(n + 1) / Fib(n + 2) * (interval.B - interval.A);
        double y1 = function.Func(x1);
        double y2 = function.Func(x2);

        for (int k = 1; k < n; k++)
        {
            if (y1 < y2)
            {
                interval = new(interval.A, x2);
                x2 = x1;
                y2 = y1;
                x1 = interval.A + Fib(n - k + 1) / Fib(n - k + 3) * (interval.B - interval.A);
                y1 = function.Func(x1);
            }
            else
            {
                interval = new(x1, interval.B);
                x1 = x2;
                y1 = y2;
                x2 = interval.A + Fib(n - k + 2) / Fib(n - k + 3) * (interval.B - interval.A);
                y2 = function.Func(x2);
            }
        }

        _min = interval.Center;

    }

    // private int Fib(int n)
    //     => n > 1 ? Fib(n - 1) + Fib(n - 2) : n;

    private double Fib(int n)
        => (Math.Pow((1 + Math.Sqrt(5)) / 2, n) -
            Math.Pow((1 - Math.Sqrt(5)) / 2, n)) / Math.Sqrt(5);
}