namespace XUnitTest.APP;

public class CalculatorService:ICalculatorService
{
    public int add(int a, int b)
    {
        if (a == 0 || b == 0)
        {
            throw  new ArgumentException("Values cannot be zero",paramName:"zero");
        }
        return a + b;
    }
}