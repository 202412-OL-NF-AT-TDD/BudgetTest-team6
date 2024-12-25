using FluentAssertions;
using NSubstitute;

namespace BudgetTest;

public class BudgetTests
{
    private BudgetService _target;
    private IBudgetRepo _budgetRepo;

    [SetUp]
    public void Setup()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _target = new BudgetService(_budgetRepo);
    }
    

    [Test]
    public void Invalid_DateRange()
    {
        var start = new DateTime(2024, 12, 1);
        var end = new DateTime(2024, 11, 1);
        QueryShouldBe(start, end, 0);
    }

    [Test]
    public void Query_FullMonth_RepoWithData()
    {
        var start = new DateTime(2024, 12, 1);
        var end = new DateTime(2024, 12, 31);

        var budgetData = new List<Budget>
        {
            GenerateBudget("202412", 31000)
        };
        FakeBudgetRepoGetAllReturns(budgetData);
        QueryShouldBe(start, end, 31000);
    }

    [Test]
    public void Query_SameMonthPartialDays_RepoWithData()
    {
        var start = new DateTime(2024, 12, 1);
        var end = new DateTime(2024, 12, 10);

        var budgetData = new List<Budget>
        {
            GenerateBudget("202411", 60000),
            GenerateBudget("202412", 31000)
        };
        FakeBudgetRepoGetAllReturns(budgetData);
        QueryShouldBe(start, end, 10000);
    }

    [Test]
    public void Query_CrossFullMonth_RepoWithData()
    {
        var start = new DateTime(2024, 11, 1);
        var end = new DateTime(2024, 12, 31);

        var budgetData = new List<Budget>
        {
            GenerateBudget("202411", 60000),
            GenerateBudget("202412", 31000)
        };
        FakeBudgetRepoGetAllReturns(budgetData);
        QueryShouldBe(start, end, 91000);
    }

    [Test]
    public void Query_CrossPartialMonth_RepoWithData()
    {
        var start = new DateTime(2024, 10, 25);
        var end = new DateTime(2024, 12, 5);

        var budgetData = new List<Budget>
        {
            GenerateBudget("202410", 3100),
            GenerateBudget("202411", 60000),
            GenerateBudget("202412", 31000)
        };
        FakeBudgetRepoGetAllReturns(budgetData);
        QueryShouldBe(start, end, 65700);
    }

    [Test]
    public void Query_CrossPartialMonth_RepoWithData_V2()
    {
        var start = new DateTime(2024, 10, 25);
        var end = new DateTime(2024, 11, 5);

        var budgetData = new List<Budget>
        {
            GenerateBudget("202410", 3100),
            GenerateBudget("202411", 60000),
            GenerateBudget("202412", 31000)
        };
        FakeBudgetRepoGetAllReturns(budgetData);
        QueryShouldBe(start, end, 10700);
    }

    [Test]
    public void Query_SameMonth_RepoWithNoData()
    {
        var start = new DateTime(2024, 10, 25);
        var end = new DateTime(2024, 10, 26);

        var budgetData = new List<Budget>
        {
            GenerateBudget("202411", 60000),
            GenerateBudget("202412", 31000)
        };
        FakeBudgetRepoGetAllReturns(budgetData);
        QueryShouldBe(start, end, 0);
    }

    private void FakeBudgetRepoGetAllReturns(List<Budget> budgetData)
    {
        _budgetRepo.GetAll().Returns(budgetData);
    }

    private static Budget GenerateBudget(string yearMonth, int amount)
    {
        return new Budget(yearMonth, amount);
    }

    private void QueryShouldBe(DateTime start, DateTime end, decimal expected)
    {
        var actual = _target.Query(start, end);
        actual.Should().Be(expected);
    }
}