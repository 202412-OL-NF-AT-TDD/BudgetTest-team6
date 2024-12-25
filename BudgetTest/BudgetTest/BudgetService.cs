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
        var period = new Period(start, end);
        foreach (var budget in budgets)
        {
            var overlappingAmount = OverlappingAmount(period, budget);
            totalBudget += overlappingAmount;
        }

        return totalBudget;
    }

    private static int OverlappingAmount(Period period, Budget budget)
    {
        var overlappingDays =
            period.OverlappingDays(budget.CreatePeriod());

        var overlappingAmount = budget.DailyAmount() * overlappingDays;
        return overlappingAmount;
    }
}