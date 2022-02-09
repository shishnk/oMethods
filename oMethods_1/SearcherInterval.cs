namespace oMethods_1;

public static class SearcherInterval
{
    public static Interval Search(IFunction function, double x0, double sigma)
    {
        double x, xk, xk1, h;

        if (function.Func(x0) > function.Func(x0 + sigma))
        {
            xk = x0 + sigma;
            h = sigma;
        }
        else
        {
            xk = x0 - sigma;
            h = -sigma;
        }

        do
        {
            h *= 2;
            xk1 = xk + h;
            x = x0;
            x0 = xk;
            xk = xk1;

        } while (function.Func(x0) > function.Func(xk));

        return (x < xk1) ? new(x, xk1) : new(xk1, x);
    }
}