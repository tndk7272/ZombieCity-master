using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : GaugeUI<HealthUI>
{ }
public class GaugeUI<T> : SingletonMonoBehavior<T>
where T : SingletonBase
{
    protected TextMeshProUGUI valueText;
    public Image[] images; // 8
    public Sprite enable, current, disable;
    protected override void OnInit()
    {
        valueText = transform.Find("ValueText")
            .GetComponent<TextMeshProUGUI>();
    }
    internal void SetGauge(int value, int maxValue) // 50, 100
    {
        valueText.text = $"{value}/{maxValue}";


        float percent = (float)value / maxValue; // 0.5 * images.Length(8) = 4
        int currentCount = Mathf.RoundToInt(percent * images.Length) - 1;
        for (int i = 0; i < images.Length; i++)
        {
            if (i == currentCount)
                images[i].sprite = current;
            else if (i < currentCount)
                images[i].sprite = enable;
            else
                images[i].sprite = disable;
        }
    }
    protected IEnumerator SetAnimateGaugeCo(int value
        , int maxValue, float duration)
    {
        foreach (var item in images)
            item.sprite = disable;

        float timePerEach = duration / images.Length;

        float percent = (float)value / maxValue; // 0.5 * images.Length(8) = 4
        int currentCount = Mathf.RoundToInt(percent * images.Length) - 1;

        for (int i = 0; i < images.Length; i++)
        {
            if (i == currentCount)
                images[i].sprite = current;
            else if (i < currentCount)
                images[i].sprite = enable;
            else
                images[i].sprite = disable;

            yield return new WaitForSeconds(timePerEach);
        }
    }
}
