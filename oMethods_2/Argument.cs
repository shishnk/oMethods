namespace oMethods_2;

public class ArgumentConverter : JsonConverter {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Argument arg = (Argument)value;

        writer.WriteStartObject();
        writer.WritePropertyName("Extremum");
        writer.WriteStartArray();
        Array.ForEach(arg.Variables, (x) => writer.WriteValue(x));
        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        Argument value;
        
        var maintoken = JObject.Load(reader);
        var token = maintoken["Number"];
        int number = Convert.ToInt32(token);

        value = new(number);

        token = maintoken["Point"];
        value.Variables = JsonConvert.DeserializeObject<double[]>(token.ToString());
        
        return value;
    }

    public override bool CanConvert(Type objectType)
        => objectType == typeof(Argument);
}

[JsonConverter(typeof(ArgumentConverter))]
public class Argument : ICloneable {
    [JsonProperty("Point")]
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

    public override bool Equals(Object obj) {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            return false;
        else {
            Argument arg = (Argument)obj;
            return (this.Number == arg.Number) && (this.Variables.OrderBy(x => x).SequenceEqual(arg.Variables.OrderBy(x => x)));
        }
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

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}