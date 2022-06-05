namespace oMethods_4;

public class Solver {
    public class SolverBuilder {
        private readonly Solver _solver = new();

        public SolverBuilder SetFunction(IFunction function) {
            _solver._function = function;
            return this;
        }

        public SolverBuilder SetMethod(IMinMethod2D method) {
            _solver._method = method;
            return this;
        }

        public SolverBuilder SetArea(Rectangle rectangle) {
            _solver._rectangle = rectangle;
            return this;
        }

        public static implicit operator Solver(SolverBuilder builder)
            => builder._solver;
    }

    private IFunction _function = default!;
    private IMinMethod2D _method = default!;
    private Rectangle _rectangle = default!;

    public void Compute() {
        try {
            _method.Compute(_rectangle, _function);
        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
        }
    }

    public static SolverBuilder CreateBuilder()
        => new();
}