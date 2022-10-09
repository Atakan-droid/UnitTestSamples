namespace XUnitTest.APP;

public class Calculator
{
    private readonly ICalculatorService _calculator;

    public Calculator(ICalculatorService calculator)
    {
        _calculator = calculator;
    }

    public int add(int a, int b)
    {
        return _calculator.add(a, b);
    }
}