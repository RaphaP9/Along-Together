using UnityEngine;

[System.Serializable]
public class DataModeledCharacterData
{
    public int characterID;
    [Space]
    public int runsPlayed;
    public int runsWon;
    public int runsLost;

    public DataModeledCharacterData(int characterID, int runsPlayed, int runsWon, int runsLost)
    {
        this.characterID = characterID;
        this.runsPlayed = runsPlayed;
        this.runsWon = runsWon;
        this.runsLost = runsLost;
    }
}
