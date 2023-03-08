using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatHandler : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI GoldNum;
    public void SetColour(Color color)
    {
        slider.transform.GetChild(0).GetComponent<Image>().color = color;
    }
    public void SetRepLevel(int Faction)
    {
        Text.text = GetComponent<StatsStorage>().RepLevel[Faction].ToString();
    }
    public void UpdateMoney()
    {
        GoldNum.text = GetComponent<StatsStorage>().Currency.ToString();
    }
}
