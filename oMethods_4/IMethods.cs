namespace oMethods_4;

public interface IMinMethod2D {
    public Vector2D? Min { get; }
    public double Eps { get; init; }

    public void Compute(IFunction function, Vector2D initPoint);
}

public interface ISearchMethod2D {
    public Vector2D? Min { get; }
    public double Eps { get; init; }

    public void Compute(Rectangle rectangle, IFunction function);
}