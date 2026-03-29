using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveExample : MonoBehaviour, IDataPersistence
{
    #region Variables

    [SerializeField] private int ParasiteID = 0;
    
    private int MagicPoint = 10;
    private int MagicPointLimit = 10;
    
    private List<bool> ParasiteAliveStates = new List<bool>();

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
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            KillParasite(ParasiteID);
        }
    }

    #endregion

    #region TestingFunctions

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

    public void KillParasite(int parasiteIndex)
    {
        if (parasiteIndex >= 0 && parasiteIndex < ParasiteAliveStates.Count)
        {
            ParasiteAliveStates[parasiteIndex] = false;
        }
    }

    #endregion

    private void UpdateText()
    {
        TestText.text = "" + MagicPoint;
    }

    #region SaveThing

    public void LoadData(GameData data)
    {
        this.MagicPoint = data.EnergyPoint;
        this.ParasiteAliveStates = new List<bool>(data.ParasitesAlive);
    }

    public void SaveData(ref GameData data)
    {
        data.EnergyPoint = this.MagicPoint;
        data.ParasitesAlive = new List<bool>(this.ParasiteAliveStates);
    }

    #endregion
}