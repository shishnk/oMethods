namespace oMethods_3;

public interface IFunction {
    public double Value(Argument arg);
    public double PenaltyValue(Argument arg);
    public bool Limitation(Argument arg);
}

public class FunctionA : IFunction {
    public double Value(Argument arg)
        => 5 * (arg[0] + arg[1]) * (arg[0] + arg[1]) +
           (arg[0] - 2) * (arg[0] - 2);

    public double PenaltyValue(Argument arg)
            => 0.5 * (arg[0] - arg[1] + Math.Abs(arg[0] - arg[1]));

    public bool Limitation(Argument arg)
        => arg[0] + arg[1] - 1 >= 0;
}

public class FunctionB : IFunction {
    public double Value(Argument arg)
        => 5 * (arg[0] + arg[1]) * (arg[0] + arg[1]) +
           (arg[0] - 2) * (arg[0] - 2);

   public double PenaltyValue(Argument arg)
        => Math.Abs(arg[0] + arg[1] - 1);

    public bool Limitation(Argument arg)
        => arg[0] - arg[1] == 0;
}