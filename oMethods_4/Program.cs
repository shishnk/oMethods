using oMethods_4;

Solver solver = Solver.CreateBuilder()
.SetFunction(new Function())
.SetMethod(new SimpleRandomSearch(1E-10))
.SetArea(Rectangle.ReadJson("area.json")!.Value);

solver.Compute();