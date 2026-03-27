using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage Config")] 
    [SerializeField] private string fileName; 
    [SerializeField] private bool useEncryption;
    
    private GameData gameData;
    
    private List<IDataPersistence> dataPersistencesObjects;
    private FileDataHandler dataHandler;
    
    public static DataPersistenceManager DataPM_instance { get; private set; }

    
    
    #region UnityLifeCycle

    private void Awake()
    {

        if (DataPM_instance != null)
        {
            Debug.LogError("There's more than one Data Persistence Manage in the scene !");
        }
        DataPM_instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataPersistencesObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    #endregion

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //Load any saved data from file using DataHandler
        this.gameData = dataHandler.Load();
        
        //if it don't work create a new one
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Creating new one ...");
            NewGame();
        }
        
        
        //TODO - push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //TODO - Pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        
        //save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {

        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    
}
