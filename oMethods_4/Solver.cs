namespace oMethods_4;

public class Solver {
    public class SolverBuilder {
        private readonly Solver _solver = new();

        public SolverBuilder SetFunction(IFunction function) {
            _solver._function = function;
            return this;
        }

        public SolverBuilder SetMethod(ISearchMethod2D method) {
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
    private ISearchMethod2D _method = default!;
    private Rectangle _rectangle = default!;

    public void Compute() {
        try {
            ArgumentNullException.ThrowIfNull(_function,
                                              $"{nameof(_function)} cannot be null, set the function");

             ArgumentNullException.ThrowIfNull(_rectangle,
                                              $"{nameof(_rectangle)} cannot be null, set the area");

            ArgumentNullException.ThrowIfNull(_method,
                                              $"{nameof(_method)} cannot be null, set the method of minimization");

            _method.Compute(_rectangle, _function);
        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
        }
    }

    public static SolverBuilder CreateBuilder()
        => new();
}