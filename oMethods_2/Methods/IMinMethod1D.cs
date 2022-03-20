namespace oMethods_2;

public interface IMinMethod1D {
    public double Min { get; }
    public double Eps { get; init; }

    public void Compute(Interval interval, IFunction function, Argument argument, Argument direction);
}