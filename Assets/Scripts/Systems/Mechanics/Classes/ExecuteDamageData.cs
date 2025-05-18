public class ExecuteDamageData
{
    private const int EXECUTE_DAMAGE = 999;

    public bool isCrit;
    public IDamageSourceSO damageSource;
    public bool triggerHealthTakeDamageEvents;
    public bool triggerShieldTakeDamageEvents;

    public int executeDamage;

    public ExecuteDamageData(bool isCrit, IDamageSourceSO damageSource, bool triggerHealthTakeDamageEvents, bool triggerShieldTakeDamageEvents)
    {
        this.isCrit = isCrit;
        this.damageSource = damageSource;
        this.triggerHealthTakeDamageEvents = triggerHealthTakeDamageEvents;
        this.triggerShieldTakeDamageEvents = triggerShieldTakeDamageEvents;
        executeDamage = EXECUTE_DAMAGE;
    }
}
