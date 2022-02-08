namespace oMethods_1;

public class Solver
{
    private Interval interval;
    private IFunction _function;
    private IMinMethod _method;
    private double eps;
    private double sigma;

    public Solver(string pathParameters)
    {
        try
        {
            using (var sr = new StreamReader(pathParameters))
            {
                interval = Interval.Parse(sr.ReadLine());
                eps = double.Parse(sr.ReadLine());
                sigma = double.Parse(sr.ReadLine());

                if (sigma > eps)
                    throw new Exception("The value of sigma must be less than the value of epsilon!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void SetFunction(IFunction function)
        => _function = function;

    public void SetMethod(IMinMethod method)
        => _method = method;

    public void Compute()
    {
        try
        {
            if (_function is null)
                throw new Exception("Set the function!");
            
            if (_method is null)
                throw new Exception("Set the method of minimization!");

            _method.Compute(interval, _function, eps, sigma);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public void WriteToFile(string path)
    {
        using (var sw = new StreamWriter(path))
        {
            sw.WriteLine(_method.Min);
        }
    }
}