using oMethods_2;

Solver solver = new("point1.json");
solver.SetFunction(new Function());
// solver.SetMinMethod(new SimplexMethod(200, 1, 1E-7));
solver.SetMinMethod(new BFGS(200, 1E-4));
solver.Compute();

// ScriptEngine engine = Python.CreateEngine(); // IronPython не поддерживает некоторые пакеты Python(к примеру numpy-> грустно :(  ))
// engine.ExecuteFile("graphics.py");