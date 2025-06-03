public class ShieldData
{
    public int shieldAmount;
    public IShieldSourceSO shieldSource;

    public ShieldData(int shieldAmount, IShieldSourceSO shieldSource)
    {
        this.shieldAmount = shieldAmount;
        this.shieldSource = shieldSource;
    }
}
