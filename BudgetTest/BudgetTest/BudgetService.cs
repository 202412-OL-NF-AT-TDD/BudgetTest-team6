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
            var budgetFirstDay = budget.FirstDay();
            var budgetEndDay = budgetFirstDay.AddMonths(1).AddDays(-1);

            var startBegin = new DateTime(start.Year, start.Month, 1);

            if (budgetFirstDay >= start && budgetEndDay <= end)
            {
                totalBudget += budget.Amount;
            }
            else if (budgetFirstDay == startBegin)
            {
                var daysSpan = (budgetEndDay - start).Days + 1;
                var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
                var result = budget.Amount / daysInMonth * daysSpan;
                totalBudget += result;
            }
            else if (budget.YearMonth == end.ToString("yyyyMM"))
            {
                var daysSpan = end.Day;
                var daysInMonth = DateTime.DaysInMonth(end.Year, end.Month);
                var result = budget.Amount / daysInMonth * daysSpan;
                totalBudget += result;
            }
        }

        return totalBudget;
    }
}