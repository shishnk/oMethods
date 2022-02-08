namespace oMethods_1;

public interface IMinMethod
{
    public double Min { get; }
    public void Compute(Interval interval, IFunction function, double eps, double sigma);
}