namespace BudgetTest;

public class Period
{
    public Period(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public DateTime End { get; private set; }

    public DateTime Start { get; private set; }

    public int OverlappingDays(Budget budget)
    {
        if (End < budget.FirstDay() || Start > budget.LastDay())
        {
            return 0;
        }

        DateTime overlappingEnd;
        DateTime overlappingStart;
        if (budget.YearMonth == Start.ToString("yyyyMM"))
        {
            overlappingEnd = budget.LastDay();
            overlappingStart = Start;
        }
        else if (budget.YearMonth == End.ToString("yyyyMM"))
        {
            overlappingEnd = End;
            overlappingStart = budget.FirstDay();
        }
        else
        {
            overlappingEnd = budget.LastDay();
            overlappingStart = budget.FirstDay();
        }

        var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
        return overlappingDays;
    }
}

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        if (end < start)
        {
            return 0m;
        }

        var budgets = _budgetRepo.GetAll();

        if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
        {
            var yearMonth = start.ToString("yyyyMM");
            var budget = budgets.FirstOrDefault(budget => budget.YearMonth == yearMonth);

            if (budget == null)
            {
                return 0m;
            }

            var amount = budget.Amount;
            var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
            var days = (end - start).Days + 1;
            var result = amount / daysInMonth * days;

            return result;
        }

        var totalBudget = 0m;
        foreach (var budget in budgets)
        {
            // if (end < budget.FirstDay() || start > budget.LastDay())
            // {
            //     continue;
            // }

            var overlappingDays = new Period(start, end).OverlappingDays(budget);

            totalBudget += budget.DailyAmount() * overlappingDays;
        }

        return totalBudget;
    }
}