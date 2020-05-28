using Moq;
using SimpleBank.Controllers;
using Xunit;

namespace SimpleBank.Test
{
    public class CalculatorTests
    {
        private DashboardController _dashboardController = new DashboardController();

        [Theory]
        [InlineData(4, 5, 9)]
        [InlineData(0, 5, 5)]
        public void Add_ShouldAddSimpleValues(int x, int y, int expected)
        {
            // act
            int actual = _dashboardController.Add(x, y);

            // assert
            Assert.Equal(actual, expected);
        }
    }
}
