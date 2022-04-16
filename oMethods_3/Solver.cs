namespace oMethods_3;

public enum MethodTypes {
    Penalty,
    InteriorPoint
}

public class Solver {
    public class SolverBuilder {
        private readonly Solver _solver = new();

        public SolverBuilder SetPoint(Argument point) {
            _solver._initPoint = point;
            return this;
        }

        public SolverBuilder SetFunction(IFunction function) {
            _solver._function = function;
            return this;
        }

        public SolverBuilder SetMinMethod(IMinMethodND method) {
            _solver._methodND = method;
            return this;
        }

        public SolverBuilder SetMinMethod1D(IMinMethod1D method) {
            _solver._method1D = method;
            return this;
        }

        public SolverBuilder SetMethodType(MethodTypes type) {
            _solver._type = type;
            return this;
        }

        public SolverBuilder SetPenaltyCoef(double coef) {
            _solver._coef = coef;
            return this;
        }

        public static implicit operator Solver(SolverBuilder builder)
            => builder._solver;
    }

    private IFunction _function = default!;
    private IMinMethodND _methodND = default!;
    private IMinMethod1D _method1D = default!;
    private Argument _initPoint = default!;
    private MethodTypes _type;
    private double _coef;

    public void Compute() {
        try {
            ArgumentNullException.ThrowIfNull(_initPoint, $"{nameof(_initPoint)} cannot be null, set the init point");

            ArgumentNullException.ThrowIfNull(_function,
                                              $"{nameof(_function)} cannot be null, set the function");

            ArgumentNullException.ThrowIfNull(_methodND,
                                              $"{nameof(_methodND)} cannot be null, set the method of minimization");

            if (_methodND.Need1DSearch && _method1D is null)
                throw new ArgumentNullException(nameof(_method1D), "Set the one dimensional search method");

            if (!_function.Limitation(_initPoint))
                throw new Exception("The starting point does not satisfy the limitation of the function");

            _methodND.Compute(_initPoint, _function, _method1D, _type, _coef);

        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
        }
    }

    public static SolverBuilder CreateBuilder()
        => new();
}