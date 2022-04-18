namespace oMethods_3;

public enum MethodTypes {
    Penalty,
    InteriorPointLog,
    InteriorPointReverse
}

public enum StrategyTypes {
    Add,
    Mult,
    Div,
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

        public SolverBuilder SetStrategyType((StrategyTypes, double) strategy) {
            _solver._strategy = strategy;
            return this;
        }

        public static implicit operator Solver(SolverBuilder builder)
            => builder._solver;
    }

    private IFunction _function = default!;
    private IMinMethodND _methodND = default!;
    private IMinMethod1D _method1D = default!;
    private Argument _initPoint = default!;
    private (StrategyTypes, double) _strategy;

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

            if (_function.Degree is null && _function.MethodType == MethodTypes.Penalty)
                throw new Exception("When selecting the penalty function method, the value of the degree cannot be null");

            if (_function.MethodType == MethodTypes.Penalty && _strategy.Item1 != StrategyTypes.Add && _strategy.Item1 != StrategyTypes.Mult)
                throw new Exception("With the chosen method of penalty functions, the strategy for changing the coefficient should be addition or multiplication");

            if ((_function.MethodType == MethodTypes.InteriorPointLog || _function.MethodType == MethodTypes.InteriorPointReverse) &&
                _strategy.Item1 != StrategyTypes.Div)
                throw new Exception("With the chosen method of barrier functions, the strategy for changing the coefficient should be division");

            if (_function.MethodType == MethodTypes.Penalty && _function.Degree != 1 && (_function.Degree % 2 != 0 || _function.Degree == 0))
                throw new Exception("The degree of the penalty function must be even or equal to one");

            _methodND.Compute(_initPoint, _function, _method1D, _strategy);

        } catch (Exception ex) {
            Console.WriteLine($"We had problem: {ex.Message}");
        }
    }

    public static SolverBuilder CreateBuilder()
        => new();
}