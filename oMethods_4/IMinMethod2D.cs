namespace oMethods_4;

public interface IMinMethod2D {
    public Vector2D? Min { get; }
    public double Eps { get; init; }

    public void Compute(Rectangle rectangle, IFunction function);
}