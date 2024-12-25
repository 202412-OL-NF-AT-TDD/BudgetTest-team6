using FluentAssertions;
using NSubstitute;

namespace BudgetTest;

public class BudgetTests
{
    private BudgetService _target;
    private IBudgetRepo? _budgetRepo;

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

    private void QueryShouldBe(DateTime start, DateTime end, decimal expected)
    {
        var actual = _target.Query(start, end);
        actual.Should().Be(expected);
    }
}