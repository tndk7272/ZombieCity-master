using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HealthUI : SingletonMonoBehavior<HealthUI>
{
    TextMeshProUGUI valueText;
    // 이미지들 배열
    public Image[] Images;  
    public Sprite enable, current, disable;

    protected override void OnInit()
    {
        valueText = transform.Find("ValueText").GetComponent<TextMeshProUGUI>();
    }

    internal void SetHp(int hp, int maxHp)
    {  
        valueText.text = $"{hp}/{maxHp}";

        float percent = (float)hp / maxHp; // 결과값이 int 로 나오기 때문에 둘 중 하나를 float 으로 바꿔줘야함
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
