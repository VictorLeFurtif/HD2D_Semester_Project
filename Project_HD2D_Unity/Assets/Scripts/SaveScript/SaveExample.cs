using System;
using TMPro;
using UnityEngine;

public class SaveExample : MonoBehaviour,IDataPersistence
{

    #region Variables

    private int MagicPoint = 10;
    private int MagicPointLimit = 10;

    #endregion

    #region Link

    public TMP_Text TestText;

    #endregion

    #region UnityLifeCycle

    private void Start()
    {
        UpdateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddPoint();
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            RemovePoint();
        }
    }

    #endregion


    private void AddPoint()
    {
        if (MagicPoint + 1 <= MagicPointLimit)
        {
            MagicPoint++;
        }
        UpdateText();
    }
    
    private void RemovePoint()
    {
        if (MagicPoint - 1 >= 0)
        {
            MagicPoint--;
        }
        UpdateText();
    }

    private void UpdateText()
    {

        TestText.text = "" + MagicPoint;

    }
    
    public void LoadData(GameData data)
    {
        this.MagicPoint = data.EnergyPoint;
    }

    public void SaveData(ref GameData data)
    {
        data.EnergyPoint = this.MagicPoint;
    }
}
