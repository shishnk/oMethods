using oMethods_4;

Solver solver = Solver.CreateBuilder()
.SetFunction(new Function())
.SetMethod(new SimpleRandomSearch(0.1, 0.999))
.SetArea(Rectangle.ReadJson("area.json")!.Value);

solver.Compute();