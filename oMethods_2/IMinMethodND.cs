namespace oMethods_2;

public interface IMinMethodND {
    public Point2D Min { get; }
    public double Eps { get; init; }

    public void Compute(Point2D initPoint, IFunction function);
}