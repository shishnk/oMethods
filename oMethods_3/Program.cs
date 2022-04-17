using oMethods_3;

Solver solver = Solver.CreateBuilder().SetPoint(Argument.ReadJson("point.json")!).
SetFunction(new FunctionA(2, 1)).SetMinMethod(new GaussAlgorithm(10000, 1E-03)).
SetMinMethod1D(new Fibonacci(1E-07)).SetStrategyType(StrategyTypes.Multiply);

solver.Compute();