namespace oMethods_1;

public class Function : IFunction
{
    public double Func(double point)
    => (point - 8) * (point - 8);
}