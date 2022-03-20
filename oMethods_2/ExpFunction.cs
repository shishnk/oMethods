namespace oMethods_2;

public class ExpFunction : IFunction {
    public double Value(Argument arg) // Maximum
        => -(3 * Math.Exp(-(arg[0] - 2) * (arg[0] - 2) - ((arg[1] - 3) / 2) * ((arg[1] - 3) / 2)) +
        Math.Exp(-((arg[0] - 1) / 2) * ((arg[0] - 1) / 2) - (arg[1] - 1) * (arg[1] - 1)));
}