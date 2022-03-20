namespace oMethods_2;

public class Solver {
    private IFunction _function;
    private IMinMethodND _methodND;
    private IMinMethod1D _method1D;
    private Argument _initPoint;

    public Solver(string initPointPath) {
        try {
            using (var sr = new StreamReader(initPointPath)) {
                _initPoint = JsonConvert.DeserializeObject<Argument>(sr.ReadToEnd());
            }

        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
        }
    }

    public void Compute() {
        try {
            if (_function is null)
                throw new ArgumentNullException(nameof(_function), "Set the function!");

            if (_methodND.Need1DSearch && _method1D is null)
                throw new ArgumentNullException(nameof(_method1D), "Set the one dimensional search method!");

            if (_methodND is null)
                throw new ArgumentNullException(nameof(_methodND), "Set the method of minimization!");

            _methodND.Compute(_initPoint, _function);

        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
        }
    }

    public void SetFunction(IFunction function)
        => _function = function;

    public void SetMinMethod1D(IMinMethod1D method)
        => _method1D = method;

    public void SetMinMethod(IMinMethodND method)
        => _methodND = method;
}