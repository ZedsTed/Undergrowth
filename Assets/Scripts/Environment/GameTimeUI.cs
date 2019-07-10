using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimeUI : MonoBehaviour
{
    public TextMeshProUGUI DayCount;

    public TextMeshProUGUI Hour;
    public TextMeshProUGUI Minute;

    protected void Update()
    {
        DayCount.text = GameTime.Instance.DayCount.ToString();

        Hour.text = (GameTime.Instance.MinuteCount / 60).ToString("D2");
        Minute.text = (GameTime.Instance.MinuteCount % 60).ToString("D2");
    }
}
