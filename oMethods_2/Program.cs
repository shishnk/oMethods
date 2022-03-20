using oMethods_2;

Solver solver = new("initPoint.json");
solver.SetFunction(new Polynom());
solver.SetMinMethod(new SimplexMethod(200, 1, 1E-7));
// solver.SetMinMethod(new BFGS(200, 1E-4));
// solver.SetMinMethod1D(new Fibonacci(1E-4));
solver.Compute();

// ScriptEngine engine = Python.CreateEngine(); // IronPython не поддерживает некоторые пакеты Python(к примеру numpy-> грустно :(  ))
// engine.ExecuteFile("graphics.py");