namespace oMethods_3;

public class ArgumentConverter : JsonConverter {
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
        if (value is null) {
            writer.WriteNull();
            return;
        }

        Argument arg = (Argument)value;

        writer.WriteStartObject();
        writer.WritePropertyName("Extremum");
        writer.WriteStartArray();
        Array.ForEach(arg.Variables, (x) => writer.WriteValue(x));
        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null || reader.TokenType != JsonToken.StartObject)
            return null;

        Argument value;

        JObject? maintoken = JObject.Load(reader);
        JToken? token = maintoken["Number"];
        int number = Convert.ToInt32(token);

        value = new(number);

        token = maintoken["Point"];

        if (token is not null) {
            double[]? variables = serializer.Deserialize<double[]>(token.CreateReader());

            if (variables is not null)
                value.Variables = variables;
        }

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

    public double Norm() {
        double result = 0.0;

        for (int i = 0; i < Number; i++)
            result += Variables[i] * Variables[i];

        return Math.Sqrt(result);
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

    public static Argument? ReadJson(string jsonPath) {
        try {
            if (!File.Exists(jsonPath))
                throw new Exception("File does not exist");

            var sr = new StreamReader(jsonPath);
            using (sr) {
                return JsonConvert.DeserializeObject<Argument>(sr.ReadToEnd());
            }

        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
        }

        return null;
    }

    public object Clone() {
        Argument arg = new(Number);
        Array.Copy(Variables, arg.Variables, Number);

        return arg;
    }
}