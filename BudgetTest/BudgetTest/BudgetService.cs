namespace BudgetTest;

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
            if (end < budget.FirstDay() || start > budget.LastDay())
            {
                continue;
            }

            DateTime overlappingEnd;
            DateTime overlappingStart;
            if (budget.YearMonth == start.ToString("yyyyMM"))
            {
                overlappingEnd = budget.LastDay();
                overlappingStart = start;
                // overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            }
            else if (budget.YearMonth == end.ToString("yyyyMM"))
            {
                overlappingEnd = end;
                overlappingStart = budget.FirstDay();
                // overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            }
            else
            {
                overlappingEnd = budget.LastDay();
                overlappingStart = budget.FirstDay();
                // overlappingDays = (overlappingEnd - overlappingStart).Days + 1;
            }

            var overlappingDays = (overlappingEnd - overlappingStart).Days + 1;

            totalBudget += budget.DailyAmount() * overlappingDays;
        }

        return totalBudget;
    }
}