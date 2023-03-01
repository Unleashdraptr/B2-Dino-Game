using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private string fileName;
    private GameData gameData;
    public static DataManager Instance { get; private set; }
    private FileConverter dataHandler;
    private List<IDataHandler> dataHandlerobjects;

    //Checks if there is another one of the managers and removes it if so
    public void Awake()
    {
        Instance = this;
    }
    //Gets all the objects it will need to write data to at the beginning
    private void Start()
    {
        dataHandler = new FileConverter(Application.persistentDataPath, fileName);
        dataHandlerobjects = FindAllDataHandlerObjects();
        LoadGame();
    }
    //If a new game wipe it clean
    public void NewGame()
    {
        gameData = new GameData();
    }
    //Gets all the relevent data it will need and then stores then within the external file
    public void SaveGame()
    {
        foreach (IDataHandler dataHandlerObj in dataHandlerobjects)
        {
            dataHandlerObj.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }
    //Gets the data from the file and applies it to the relevent data
    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if (gameData == null)
        {
            NewGame();
        }
        foreach (IDataHandler dataHandlerObj in dataHandlerobjects)
        {
            dataHandlerObj.LoadData(gameData);
        }    
    }
    //If the player quits it will save
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    //Findas all the needed objects and puts then in a list
    private List<IDataHandler> FindAllDataHandlerObjects()
    {
        IEnumerable<IDataHandler> dataHandlers = FindObjectsOfType<MonoBehaviour>().OfType<IDataHandler>();
        return new List<IDataHandler>(dataHandlers);
    }
}
