using Moq;
using XUnitTest.APP;

namespace UdemyUnitTest.Test;

public class Mocker
{
    public Mock<ICalculatorService> _mockCalculator { get; set; }

    public Mocker()
    {
        Create_Mock_CalculatorService();
    }

    private void Create_Mock_CalculatorService()
    {
        _mockCalculator = new Mock<ICalculatorService>();
    }
}