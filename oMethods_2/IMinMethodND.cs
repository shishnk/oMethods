namespace oMethods_2;

public interface IMinMethodND {
    public Argument Min { get; }
    public double Eps { get; init; }
    public bool Need1DSearch { get; }

    public void Compute(Argument initPoint, IFunction function);
}