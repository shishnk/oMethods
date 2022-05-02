namespace oMethods_3;

public interface IFunction {
    public double Coef { get; set; }
    public int? Degree { get; init; }
    public MethodTypes? MethodType { get; init; }
    public double Value(Argument arg);
    public double PenaltyValue(Argument arg);
    public bool Limitation(Argument arg);
}

public class FunctionA : IFunction {
    public double Coef { get; set; }
    public int? Degree { get; init; }
    public MethodTypes? MethodType { get; init; }

    public FunctionA(double? coef, int? degree, MethodTypes? methodType)
        => (Coef, Degree, MethodType) = (coef ?? 0, degree, methodType ?? MethodTypes.Penalty);

    public double Value(Argument arg)
        => 5 * (arg[0] + arg[1]) * (arg[0] + arg[1]) +
           (arg[0] - 2) * (arg[0] - 2);

    // public double PenaltyValue(Argument arg) => MethodType switch {
    //     MethodTypes.Penalty => Coef * Math.Pow(0.5 * (-arg[0] - arg[1] + 1 + Math.Abs(-arg[0] - arg[1] + 1)), Degree!.Value),

    //     MethodTypes.InteriorPointLog => (-Coef * Math.Log(arg[0] + arg[1] - 1)).Equals(Double.NaN) ? 0
    //                                     : -Coef * Math.Log(arg[0] + arg[1] - 1),

    //     MethodTypes.InteriorPointReverse => -Coef / (arg[0] + arg[1] - 1),

    //     _ => throw new InvalidEnumArgumentException($"This type of method does not exist: {nameof(MethodType)}")
    // };

    public double PenaltyValue(Argument arg) {
        switch (MethodType) {
            case MethodTypes.InteriorPointLog:
                // Console.WriteLine($"ValuePenaltyFunction={Math.Log(arg[0] + arg[1] - 1)}");
                return -Coef * Math.Log(arg[0] + arg[1] - 1);
            case MethodTypes.InteriorPointReverse:
                // Console.WriteLine($"ValuePenaltyFunction={-1.0 / (-arg[0] - arg[1] + 1)}");
                return -Coef / (-arg[0] - arg[1] + 1);
            case MethodTypes.Penalty:
                return Coef * Math.Pow(0.5 * (-arg[0] - arg[1] + 1 + Math.Abs(-arg[0] - arg[1] + 1)), Degree!.Value);
            default:
                return 0;
        }

    }

    public bool Limitation(Argument arg)
        => -arg[0] - arg[1] + 1 <= 0;
}

public class FunctionB : IFunction {
    public double Coef { get; set; }
    public int? Degree { get; init; }
    public MethodTypes? MethodType { get; init; }

    public FunctionB(double? coef, int? degree, MethodTypes? methodType)
        => (Coef, Degree, MethodType) = (coef ?? 0, degree, methodType ?? MethodTypes.Penalty);

    public double Value(Argument arg)
        => 5 * (arg[0] + arg[1]) * (arg[0] + arg[1]) +
           (arg[0] - 2) * (arg[0] - 2);

    public double PenaltyValue(Argument arg)
         => Coef * Math.Pow(Math.Abs(arg[0] - arg[1]), Degree!.Value);

    public bool Limitation(Argument arg)
        => Math.Abs(arg[0] - arg[1]) < 1E-12;
}