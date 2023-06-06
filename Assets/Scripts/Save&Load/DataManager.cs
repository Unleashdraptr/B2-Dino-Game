using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static int DataSlot;
    [SerializeField] private string fileName;
    [SerializeField] private int fileNum;
    [SerializeField] private string FileSaveName;
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
        dataHandler = new FileConverter(Application.persistentDataPath, fileName, FileSaveName + fileNum.ToString());
        dataHandlerobjects = FindAllDataHandlerObjects();
        LoadGame(DataSlot);
    }
    public bool CheckLoad(int zero, int SaveNum)
    {
        string fullPath = Path.Combine(Application.persistentDataPath, fileName, FileSaveName + zero.ToString() + SaveNum.ToString());
        if (File.Exists(fullPath))
        {
            return true;
        }
        else
            return false;
    }
    //If a new game wipe it clean
    public void NewGame()
    {
        gameData = new GameData
        {
            Position = new(3016, 9, 3105),
            Currency = 1000
        };
    }
    //Gets all the relevent data it will need and then stores then within the external file
    public void SaveGame(int FileNum)
    {
        foreach (IDataHandler dataHandlerObj in dataHandlerobjects)
        {
            dataHandlerObj.SaveData(ref gameData);
        }
        dataHandler.Save(gameData, FileNum);
    }
    //Gets the data from the file and applies it to the relevent data
    public void LoadGame(int FileNum)
    {
        DataSlot = FileNum;
        gameData = dataHandler.Load(FileNum);
        if (gameData == null)
        {
            NewGame();
        }
        foreach (IDataHandler dataHandlerObj in dataHandlerobjects)
        {
            dataHandlerObj.LoadData(gameData);
        }    
    }
    //Findas all the needed objects and puts then in a list
    private void OnApplicationQuit()
    {
        SaveGame(DataSlot);
    }
    private List<IDataHandler> FindAllDataHandlerObjects()
    {
        IEnumerable<IDataHandler> dataHandlers = FindObjectsOfType<MonoBehaviour>().OfType<IDataHandler>();
        return new List<IDataHandler>(dataHandlers);
    }
}
