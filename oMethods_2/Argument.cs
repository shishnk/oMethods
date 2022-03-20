namespace oMethods_2;

public class Argument : ICloneable {
    public double[] Variables { get; set; }
    public int Number { get; init; }

    public double this[int index] {
        get => Variables[index];
        set => Variables[index] = value;
    }

    public Argument(int number) {
        Variables = new double[number];
        Number = number;
    }

    public void Fill(double value) {
        for (int i = 0; i < Number; i++)
            Variables[i] = value;
    }

    public override string ToString() { // для отрисовки графиков
        string result = string.Empty;

        for (int i = 0; i < Number; i++) {
            result += Variables[i];
            result += " ";
        }

        return result;
    }

    public static Argument operator -(Argument fstArg, Argument sndArg) {
        Argument result = new(fstArg.Number);

        for (int i = 0; i < result.Number; i++)
            result[i] = fstArg[i] - sndArg[i];

        return result;
    }

    public static Argument operator +(Argument fstArg, Argument sndArg) {
        Argument result = new(fstArg.Number);

        for (int i = 0; i < result.Number; i++)
            result[i] = fstArg[i] + sndArg[i];

        return result;
    }

    public static Argument operator *(double constant, Argument arg) {
        Argument result = new(arg.Number);

        for (int i = 0; i < result.Number; i++)
            result[i] = constant * arg[i];

        return result;
    }

    public object Clone() {
        Argument arg = new(this.Number);
        Array.Copy(this.Variables, arg.Variables, this.Number);

        return arg;
    }
}