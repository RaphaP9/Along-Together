public class HealData
{
    public int healAmount;
    public IHealSourceSO healSource;

    public HealData(int healAmount, IHealSourceSO healSource)
    {
        this.healAmount = healAmount;
        this.healSource = healSource;
    }
}
