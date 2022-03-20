namespace oMethods_2;

public class Polynom : IFunction {
    public double Value(Argument arg)
        => (arg[0] * arg[0] + arg[1] * arg[1]) *
            (arg[0] * arg[0] + arg[1] * arg[1]) -
            (arg[0] - 3 * arg[1]) * (arg[0] - 3 * arg[1]);
}