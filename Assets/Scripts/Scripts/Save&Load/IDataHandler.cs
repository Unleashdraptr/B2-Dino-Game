using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//What is applied to the scripts so they can access the voids and work within the system
public interface IDataHandler 
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
