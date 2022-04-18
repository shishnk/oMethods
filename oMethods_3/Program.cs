using oMethods_3;

Solver solverWithGauss = Solver.CreateBuilder().SetPoint(Argument.ReadJson("point.json")!)
.SetFunction(new FunctionA(2, 1, MethodTypes.InteriorPointReverse)).SetMinMethod(new GaussAlgorithm(1000, 1E-5))
.SetMinMethod1D(new Fibonacci(1E-5)).SetStrategyType((StrategyTypes.Div, 0.5));

Solver solverWithSimplex = Solver.CreateBuilder().SetPoint(Argument.ReadJson("point.json")!)
.SetFunction(new FunctionA(2, 1, MethodTypes.Penalty)).SetMinMethod(new SimplexMethod(1000, 1, 1E-4))
.SetStrategyType((StrategyTypes.Mult, 2));

solverWithGauss.Compute();
solverWithSimplex.Compute();