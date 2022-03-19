namespace oMethods_2;

public class Solver {
    private IFunction _function;
    private IMinMethodND _method;
    private Point2D _initPoint;

    public Solver(string initPointPath) {
        try {
            using (var sr = new StreamReader(initPointPath)) {
                _initPoint = JsonConvert.DeserializeObject<Point2D>(sr.ReadToEnd());
            }

        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
        }
    }

    public void Compute() {
        try {
            if (_function is null)
                throw new Exception("Set the function!");

            if (_method is null)
                throw new Exception("Set the method of minimization!");

            _method.Compute(_initPoint, _function);

        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
        }
    }

    public void SetFunction(IFunction function)
        => _function = function;

    public void SetMinMethod(IMinMethodND method)
        => _method = method;
}