namespace oMethods_2;

public class Function : IFunction { // 8 variant(Maximum)
    public double Value(Argument arg)
        => -(3.0 / (1 + (arg[0] - 2) * (arg[0] - 2) +
        ((arg[1] - 2) / 2) * (arg[1] - 2) / 2) +
        2.0 / (1 + ((arg[0] - 2) / 3) * ((arg[0] - 2) / 3) +
        (arg[1] - 3) * (arg[1] - 3)));
}