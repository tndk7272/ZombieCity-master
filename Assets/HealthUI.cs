using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthUI : GaugeUI<HealthUI>
{

}
public class GaugeUI<T> : SingletonMonoBehavior<T>
   where T : SingletonBase
{
    TextMeshProUGUI valueText;
    // 이미지들 배열
    public Image[] Images;  
    public Sprite enable, current, disable;

    protected override void OnInit()
    {
        valueText = transform.Find("ValueText").GetComponent<TextMeshProUGUI>();
    }

    internal void SetGauge(int value, int maxValue)
    {  
        valueText.text = $"{value}/{maxValue}";

        float percent = (float)value / maxValue; // 결과값이 int 로 나오기 때문에 둘 중 하나를 float 으로 바꿔줘야함
        int currentCount =  Mathf.RoundToInt(percent * Images.Length);
        for (int i = 0; i < Images.Length; i++)
        {
            if (i == currentCount)
                Images[i].sprite = current;
            else if (i < currentCount)
                Images[i].sprite = enable;
            else
                Images[i].sprite = disable;
        }
    }
}
