namespace oMethods_1;

public record struct Interval
{
    public double A { get; init; }
    public double B { get; init; }
    public double Center { get; init; }

    public Interval(double a, double b)
    {
        A = a;
        B = b;
        Center = (a + b) / 2;
    }

    public static Interval Parse(string intervalStr)
    {
        var data = intervalStr.Split();
        Interval interval = new(double.Parse(data[0]), double.Parse(data[1]));

        return interval;
    }

    public override string ToString()
        => $"[{A}, {B}]";
}