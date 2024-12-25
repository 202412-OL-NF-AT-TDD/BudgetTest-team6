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
}