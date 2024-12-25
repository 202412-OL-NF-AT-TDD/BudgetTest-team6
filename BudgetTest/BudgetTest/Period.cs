namespace BudgetTest;

public class Period
{
    public Period(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    private DateTime End { get; set; }

    private DateTime Start { get; set; }

    public int OverlappingDays(Budget budget)
    {
        var another = new Period(budget.FirstDay(), budget.LastDay());
        var firstDay = budget.FirstDay();
        var lastDay = budget.LastDay();
        if (End < firstDay || Start > lastDay)
        {
            return 0;
        }

        var overlappingEnd = End < lastDay
            ? End
            : lastDay;
        var overlappingStart = Start > firstDay
            ? Start
            : firstDay;

        return (overlappingEnd - overlappingStart).Days + 1;
    }
}