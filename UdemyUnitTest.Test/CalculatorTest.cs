using Moq;
using Xunit;
using XUnitTest.APP;

namespace UdemyUnitTest.Test;

public class CalculatorTest
{
    private Calculator Calculator { get; set; }
    public Mocker myMock { get; set; }
    
    public CalculatorTest()
    { 
        myMock = new Mocker();
        Calculator = new Calculator(myMock._mockCalculator.Object);
    }

    [Fact]
    public void Add_SimpleValues_Test()
    {
        //Arrange
        int a = 10;
        int b = 20;
        int expectedTotal = 30;
        
        myMock._mockCalculator.Setup(x => x.add(a,b)).Returns(expectedTotal);
        //Act
        var total = Calculator.add(a, b);
        
        //Assert
        Assert.Equal<int>(30,total);
        
        myMock._mockCalculator.Verify(x=>x.add(a,b),Times.Once);
    }
    [Fact]
    public void Add_SimpleValues_ThrowsException_Test()
    {
        //Arrange
        int a = 0;
        int b = 20;
        int expectedTotal = 30;
        
        myMock._mockCalculator.Setup(x => x.add(a,b)).Throws(new ArgumentException("Values cannot be zero",paramName:"zero"));
        //Assert
        ArgumentException exception =Assert.Throws<ArgumentException>(() => { Calculator.add(a, b);});
        
        Assert.Equal("zero",exception.ParamName);
    }
    [Fact]
    public void Add_SimpleValues_WithCallBack_Test()
    {
        //Arrange
        int a = 10;
        int b = 12;
        int actualTotal = 0;

        myMock._mockCalculator.Setup(x => x.add(It.IsAny<int>(), It.IsAny<int>()))
            .Callback<int, int>((x, y) => actualTotal = x + y);

        //Act
        Calculator.add(a, b);
        
        //Assert
        Assert.Equal(22,actualTotal);
    }
}