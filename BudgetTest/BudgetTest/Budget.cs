namespace BudgetTest;

public class Budget
{
    public string YearMonth { get; }
    public int Amount { get; }

    public Budget(string yearMonth, int amount)
    {
        YearMonth = yearMonth;
        Amount = amount;
    }

    public DateTime FirstDay()
    {
        return DateTime.ParseExact(YearMonth, "yyyyMM", null);
    }

    public DateTime LastDay()
    {
        return FirstDay().AddMonths(1).AddDays(-1);
    }

    public int Days()
    {
        return LastDay().Day;
    }

    public int DailyAmount()
    {
        var dailyAmount = Amount / Days();
        return dailyAmount;
    }
}