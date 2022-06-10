namespace oMethods_2;

public record struct Interval {
    public double Left { get; init; }
    public double Right { get; init; }
    public double Center { get; }

    public Interval(double left, double right) {
        Left = left;
        Right = right;
        Center = (left + right) / 2;
    }

    public bool Contain(double point)
        => (point >= Left && point <= Right);

    public override string ToString()
        => $"[{Left}; {Right}]";
}