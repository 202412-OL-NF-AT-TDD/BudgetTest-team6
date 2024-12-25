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

            int overlappingDays;
            if (budget.YearMonth == start.ToString("yyyyMM"))
            {
                overlappingDays = (budget.LastDay() - start).Days + 1;
                // totalBudget += budget.DailyAmount() * overlappingDays;
            }
            else if (budget.YearMonth == end.ToString("yyyyMM"))
            {
                overlappingDays = (end - budget.FirstDay()).Days + 1;
                // totalBudget += budget.DailyAmount() * overlappingDays;
            }
            else
            {
                overlappingDays = (budget.LastDay() - budget.FirstDay()).Days + 1;
                // totalBudget += budget.DailyAmount() * overlappingDays;
            }

            totalBudget += budget.DailyAmount() * overlappingDays;
        }

        return totalBudget;
    }
}