namespace BudgetTest;

public class Budget
{
    public Budget(string yearMonth, int amount)
    {
        YearMonth = yearMonth;
        Amount = amount;
    }

    private int Amount { get; }
    private string YearMonth { get; }

    public decimal OverlappingAmount(Period period)
    {
        return (decimal)DailyAmount() * period.OverlappingDays(CreatePeriod());
    }

    private Period CreatePeriod()
    {
        return new Period(FirstDay(), LastDay());
    }

    private int DailyAmount()
    {
        return Amount / Days();
    }

    private int Days()
    {
        return LastDay().Day;
    }

    private DateTime FirstDay()
    {
        return DateTime.ParseExact(YearMonth, "yyyyMM", null);
    }

    private DateTime LastDay()
    {
        return FirstDay().AddMonths(1).AddDays(-1);
    }
}