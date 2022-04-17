namespace oMethods_3;

public interface IFunction {
    public double Coef { get; set; }
    public int Degree { get; init; }
    public double Value(Argument arg);
    public double PenaltyValue(Argument arg);
    public bool Limitation(Argument arg);
}

public class FunctionA : IFunction {
    public double Coef { get; set; }
    public int Degree { get; init; }

    public FunctionA(double? coef, int degree)
        => (Coef, Degree) = (coef ?? 0, degree);

    public double Value(Argument arg)
        => 5 * (arg[0] + arg[1]) * (arg[0] + arg[1]) +
           (arg[0] - 2) * (arg[0] - 2);

    public double PenaltyValue(Argument arg)
        => Math.Pow(0.5 * (-arg[0] - arg[1] + 1 + Math.Abs(-arg[0] - arg[1] + 1)), Degree) / Coef;

    public bool Limitation(Argument arg)
        => -arg[0] - arg[1] + 1 < 0;
}

public class FunctionB : IFunction {
    public double Coef { get; set; }

    public int Degree { get; init; }

    public FunctionB(double? coef, int degree)
        => (Coef, Degree) = (coef ?? 0, degree);

    public double Value(Argument arg)
        => 5 * (arg[0] + arg[1]) * (arg[0] + arg[1]) +
           (arg[0] - 2) * (arg[0] - 2);

    public double PenaltyValue(Argument arg)
         => Coef * Math.Pow(Math.Abs(arg[0] - arg[1]), Degree);

    public bool Limitation(Argument arg)
        => Math.Abs(arg[0] - arg[1]) < 1E-12;
}