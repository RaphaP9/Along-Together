[System.Serializable]
public class DataPersistentNumericStat 
{
    public string originGUID;
    public string numericStatType;
    public string numericStatModificationType;
    public float value;

    public DataPersistentNumericStat(string originGUID, string numericStatType, string numericStatModificationType, float value)
    {
        this.originGUID = originGUID;
        this.numericStatType = numericStatType;
        this.numericStatModificationType = numericStatModificationType;
        this.value = value;
    }
}
