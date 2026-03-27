using UnityEngine;

[System.Serializable]
public class GameData
{

    public int DeathCount;
    public int EnergyPoint;
    
    
    //Game start with those data when there's nothing else
    public GameData()
    {
        this.DeathCount = 0;
        this.EnergyPoint = 10;
    }


}
 