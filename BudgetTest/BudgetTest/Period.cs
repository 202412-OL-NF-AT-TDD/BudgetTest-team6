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
        if (End < budget.FirstDay() || Start > budget.LastDay())
        {
            return 0;
        }

        var overlappingEnd = End < budget.LastDay()
            ? End
            : budget.LastDay();
        var overlappingStart = Start > budget.FirstDay()
            ? Start
            : budget.FirstDay();

        return (overlappingEnd - overlappingStart).Days + 1;
    }
}