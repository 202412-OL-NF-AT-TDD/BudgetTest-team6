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

        DateTime overlappingEnd = End < budget.LastDay()
            ? End
            : budget.LastDay();
        DateTime overlappingStart;
        if (budget.YearMonth == Start.ToString("yyyyMM"))
        {
            // overlappingEnd = budget.LastDay();
            overlappingStart = Start;
        }
        else if (budget.YearMonth == End.ToString("yyyyMM"))
        {
            // overlappingEnd = End;
            overlappingStart = budget.FirstDay();
        }
        else
        {
            // overlappingEnd = budget.LastDay();
            overlappingStart = budget.FirstDay();
        }

        var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
        return overlappingDays;
    }
}