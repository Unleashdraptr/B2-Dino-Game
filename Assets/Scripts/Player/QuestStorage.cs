using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestStorage : MonoBehaviour
{
    public int QuestCount;
    public bool QuestLimit;
    public Bounty[] CurBountys;
    // Start is called before the first frame update
    void Start()
    {
        UpdateCurQuests();
    }                           
    // Update is called once per frame
    public void UpdateCurQuests()
    {
        QuestCount = CurBountys.Length;
        for (int i = 0; i < QuestCount; i++)
        {
            if (i < 3)
            {
                if (CurBountys[i] == null)
                {
                    QuestCount--;
                }
            }
            else if(i < 3 && i > 6)
            {

            }
            else if(i < 6 && i > 9)
            {

            }
        }
        if(QuestCount > 3)
        {
            QuestLimit = true;
        }
    }
}
