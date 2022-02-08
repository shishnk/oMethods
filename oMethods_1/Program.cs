using oMethods_1;

Solver solver = new("parameters.txt");
solver.SetFunction(new Function());
// solver.SetMethod(new Dichotomy());
// solver.SetMethod(new GoldenSectionSearch());
solver.SetMethod(new Fibonacci());
solver.Compute();
solver.WriteToFile("result.txt");
Console.WriteLine(SearcherInterval.Search(new Function(), 1E-7));