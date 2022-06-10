namespace oMethods_4;

public readonly record struct Rectangle {
    [JsonProperty("Left bottom")]
    public Vector2D LeftBottom { get; init; }

    [JsonProperty("Right top")]
    public Vector2D RightTop { get; init; }

    [JsonIgnore]
    public Vector2D RightBottom { get; }

    [JsonIgnore]
    public Vector2D LeftTop { get; }

    [JsonIgnore]
    public double Square { get; }

    [JsonConstructor]
    public Rectangle(Vector2D leftBottom, Vector2D rightTop) {
        LeftBottom = leftBottom;
        RightTop = rightTop;
        RightBottom = new(rightTop.X, leftBottom.Y);
        LeftTop = new(leftBottom.X, rightTop.Y);
        Square = LeftTop.Distance(rightTop) * leftBottom.Distance(RightBottom);
    }

    public bool Inside(Vector2D point)
        => point.X >= LeftBottom.X && point.X <= RightBottom.X && point.Y >= LeftBottom.Y && point.Y <= LeftTop.Y;

    public static Rectangle? ReadJson(string jsonPath) {
        try {
            if (!File.Exists(jsonPath))
                throw new Exception("File does not exist");

            var sr = new StreamReader(jsonPath);
            using (sr) {
                return JsonConvert.DeserializeObject<Rectangle>(sr.ReadToEnd());
            }
        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
            return null;
        }
    }
}