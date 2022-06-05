namespace oMethods_4;

public static class RandomExtensions {
    public static double NextDouble(this Random RandGenerator, double MinValue, double MaxValue)
        => (RandGenerator.NextDouble() * (MaxValue - MinValue)) + MinValue;
}