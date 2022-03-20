namespace oMethods_2;

public class RosenbrockFunction : IFunction {
    public double Value(Argument arg)
        => 100 * (arg[1] - arg[0] * arg[0]) * (arg[1] - arg[0] * arg[0]) +
        (1 - arg[0]) * (1 - arg[0]);
}