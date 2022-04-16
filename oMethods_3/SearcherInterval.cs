namespace oMethods_3;

public static class SearcherInterval {
    public static Interval Search(IFunction function, double x0, double delta, Argument direction, Argument argument) {
        double x, xk, xk1, h;

        if (function.Value(argument + x0 * direction) > function.Value(argument + (x0 + delta) * direction)) {
            xk = x0 + delta;
            h = delta;
        } else {
            xk = x0 - delta;
            h = -delta;
        }

        do {
            h *= 2;
            xk1 = xk + h;
            x = x0;
            x0 = xk;
            xk = xk1;

        } while (function.Value(argument + x0 * direction) > function.Value(argument + xk * direction));

        return (x < xk1) ? new(x, xk1) : new(xk1, x);
    }
}