using Inewi_Short.Entities;
using Moq;

namespace Inewi_Short.Tests
{
    public class CountExcessLeaveFromPastTests : IDisposable
    {
        public void Dispose()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        }

        [Fact]
        public void AddEmployee_WhenCalledWithOneMoreYearToTakeLeave_SetsLeaveLimitsCorrectly()
        {
            // Arrange
            var input = new StringReader("b\na\n1\n");
            var mockListOfEmployees = new Mock<ListOfEmployees>();

            //Act
            Dispose();
            using (input)
            {
                Console.SetIn(input);
                mockListOfEmployees.Object.AddEmployee();

                var employee = mockListOfEmployees.Object.Employees.FirstOrDefault();
                var dayOfJoining = employee.DayOfJoining;
                DateTime date = new(2020, 1, 1);
                var ll2020 = employee.LeaveLimits.Where(l => l.Year == 2020).FirstOrDefault();
                var ll2021 = employee.LeaveLimits.Where(l => l.Year == 2021).FirstOrDefault();
                var ll2022 = employee.LeaveLimits.Where(l => l.Year == 2022).FirstOrDefault();
                var ll2023 = employee.LeaveLimits.Where(l => l.Year == 2023).FirstOrDefault();
                var ll2024 = employee.LeaveLimits.Where(l => l.Year == 2024).FirstOrDefault();

                // Assert
                Assert.Equal("b", employee.FirstName);
                Assert.Equal(date, dayOfJoining);
                Assert.Equal(26, ll2020.Limit);
                Assert.Equal(26, ll2021.Limit);
                Assert.Equal(26, ll2022.Limit);
                Assert.Equal(26, ll2023.Limit);
                Assert.Equal(26, ll2024.Limit);
            }
        }

        [Fact]
        public void AddEmployee_WhenCalledWithTwoMoreYearsToTakeLeave_SetsLeaveLimitsCorrectly()
        {
            // Arrange
            var input = new StringReader("a\na\n2\n");
            var mockListOfEmployees = new Mock<ListOfEmployees>();

            //Act
            Dispose();
            using (input)
            {
                Console.SetIn(input);
                mockListOfEmployees.Object.AddEmployee();

                var employee = mockListOfEmployees.Object.Employees.FirstOrDefault();
                var dayOfJoining = employee.DayOfJoining;
                DateTime date = new(2020, 1, 1);
                var ll2020 = employee.LeaveLimits.Where(l => l.Year == 2020).FirstOrDefault();
                var ll2021 = employee.LeaveLimits.Where(l => l.Year == 2021).FirstOrDefault();
                var ll2022 = employee.LeaveLimits.Where(l => l.Year == 2022).FirstOrDefault();
                var ll2023 = employee.LeaveLimits.Where(l => l.Year == 2023).FirstOrDefault();
                var ll2024 = employee.LeaveLimits.Where(l => l.Year == 2024).FirstOrDefault();

                // Assert
                Assert.Equal("a", employee.FirstName);
                Assert.Equal(date, dayOfJoining);
                Assert.Equal(26, ll2020.Limit);
                Assert.Equal(26, ll2021.Limit);
                Assert.Equal(26, ll2022.Limit);
                Assert.Equal(26, ll2023.Limit);
                Assert.Equal(26, ll2024.Limit);
            }
        }

        [Theory]
        [InlineData(15, 15, 15, 15, //koniec urlopu w latach 2020, 21, 23 , 24 w styczniu
            11, 22, 26, 26, //oczekiwane rezultaty - dostępne dni urlopowe z lat poprzednich, czyli z 2020, 21, 22, 23, w roku następnym
            11, 22, 33, 37, 52)] //oczekiwane rezultaty - dostępne dni urlopowe w roku 2020, 21, 22, 23, 24
        [InlineData(5, 5, 5, 5,
            21, 26, 26, 26,
            21, 42, 47, 47, 52)]
        [InlineData(25, 25, 25, 25,
            1, 2, 3, 4,
            1, 2, 3, 4, 30)]
        [InlineData(5, 10, 15, 20,
            21, 26, 26, 26,
            21, 37, 37, 32, 52)]
        [InlineData(5, 15, 25, 31,
            21, 26, 26, 21,
            21, 32, 27, 21, 46)]
        public void AddEmployeeAndPastLeaves_WhenCalledWithOneMoreYearToTakeLeave_ReturnsAvailableLeaveCorrectly
            (int DayToOf1stLeave, int DayToOf2ndLeave, int DayToOf3rdLeave, int DayToOf4thLeave,
            int excessFrom2020, int excessFrom2021, int excessFrom2022, int excessFrom2023,
            int availableLeaveIn2020, int availableLeaveIn2021, int availableLeaveIn2022, int availableLeaveIn2023, int availableLeaveIn2024)
        {
            // Arrange
            Dispose();
            var input = new StringReader("brt\na\n1\n");
            var mockListOfEmployees = new Mock<ListOfEmployees>();

            //Act
            using (input)
            {
                Console.SetIn(input);
                mockListOfEmployees.Object.AddEmployee();

                var employee = mockListOfEmployees.Object.Employees.FirstOrDefault();
                var dayOfJoining = employee.DayOfJoining;
                DateTime date = new(2020, 1, 1);
                var ll2020 = employee.LeaveLimits.Where(l => l.Year == 2020).FirstOrDefault();
                var ll2021 = employee.LeaveLimits.Where(l => l.Year == 2021).FirstOrDefault();
                var ll2022 = employee.LeaveLimits.Where(l => l.Year == 2022).FirstOrDefault();
                var ll2023 = employee.LeaveLimits.Where(l => l.Year == 2023).FirstOrDefault();
                var ll2024 = employee.LeaveLimits.Where(l => l.Year == 2024).FirstOrDefault();
                
                Leave leave = new(1, 1)
                {
                    DateFrom = new DateTime(2020, 1, 1),
                    DateTo = new DateTime(2020, 1, DayToOf1stLeave)
                };
                mockListOfEmployees.Object.AllLeavesInStorage.AddLeave(leave);

                leave = new(1, 2)
                {
                    DateFrom = new DateTime(2021, 1, 1),
                    DateTo = new DateTime(2021, 1, DayToOf2ndLeave)
                };
                mockListOfEmployees.Object.AllLeavesInStorage.AddLeave(leave);

                leave = new(1, 3)
                {
                    DateFrom = new DateTime(2022, 1, 1),
                    DateTo = new DateTime(2022, 1, DayToOf3rdLeave)
                };
                mockListOfEmployees.Object.AllLeavesInStorage.AddLeave(leave);

                leave = new(1, 4)
                {
                    DateFrom = new DateTime(2023, 1, 1),
                    DateTo = new DateTime(2023, 1, DayToOf4thLeave)
                };
                mockListOfEmployees.Object.AllLeavesInStorage.AddLeave(leave);

                // Assert
                Assert.Equal("brt", employee.FirstName);
                Assert.Equal(date, dayOfJoining);
                Assert.Equal(26, ll2020.Limit);
                Assert.Equal(26, ll2021.Limit);
                Assert.Equal(26, ll2022.Limit);
                Assert.Equal(26, ll2023.Limit);
                Assert.Equal(26, ll2024.Limit);

                Assert.Equal(excessFrom2020, mockListOfEmployees.Object.CountExcessLeaveFromPast(employee, 2020));
                Assert.Equal(excessFrom2021, mockListOfEmployees.Object.CountExcessLeaveFromPast(employee, 2021));
                Assert.Equal(excessFrom2022, mockListOfEmployees.Object.CountExcessLeaveFromPast(employee, 2022));
                Assert.Equal(excessFrom2023, mockListOfEmployees.Object.CountExcessLeaveFromPast(employee, 2023));

                Assert.Equal(availableLeaveIn2020, mockListOfEmployees.Object.CountLeaveAvailable(employee, 2020, 0));
                Assert.Equal(availableLeaveIn2021, mockListOfEmployees.Object.CountLeaveAvailable(employee, 2021, 0));
                Assert.Equal(availableLeaveIn2022, mockListOfEmployees.Object.CountLeaveAvailable(employee, 2022, 0));
                Assert.Equal(availableLeaveIn2023, mockListOfEmployees.Object.CountLeaveAvailable(employee, 2023, 0));
                Assert.Equal(availableLeaveIn2024, mockListOfEmployees.Object.CountLeaveAvailable(employee, 2024, 0));
            }
        }
    }
}
