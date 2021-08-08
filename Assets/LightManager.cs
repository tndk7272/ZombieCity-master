using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;


// 밤의 색이랑 // 낮 색을 넣을것이다. 
public class LightManager : MonoBehaviour
{
    public Color dayColor; // 낮
    public Color nightColor; // 밤 




    [ContextMenu("SetDayLight")]
    void SetDayLight()
    {
        // 디렉셔널 라이트만 켜기
        var allLight = FindObjectsOfType<Light>();
        // 빛을 하나하나 돌면서 디렉셔널 라이트면 켜자
        foreach (var item in allLight)
        {
            if (item.type == LightType.Directional)
                item.enabled = true;
            else
                item.enabled = false;
        }
        RenderSettings.ambientLight = dayColor;
    }

    [ContextMenu("SetNightColor")]
    void SetNightColor()
    {
        // 디렉셔널 라이트만 끄자
        var allLight = FindObjectsOfType<Light>();
        // 빛을 하나하나 돌면서 디렉셔널 라이트면 끄자
        foreach (var item in allLight)
        {
            if (item.type == LightType.Directional)
                item.enabled = false;
            else
                item.enabled = true;
        }
        RenderSettings.ambientLight = nightColor;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
            ChangeDayLight();
        if (Input.GetKeyDown(KeyCode.Alpha6))
            ChangeNightLight();
     
    }

    

    public float changeDuration = 3;
    // 모든 라이트의 인ㅌㅌㅌㅌㅌ를 저장할것임
    Dictionary<Light, float> allLight;
    private void ChangeDayLight()
    {
        if (allLight == null)
        {
            allLight = new Dictionary<Light, float>(); // 초기화 해주는 코드
            var _allLight = FindObjectsOfType<Light>();
            foreach (var item in _allLight)
            {
                allLight[item] = item.intensity;
            }
        }
        // 낮으로 변함 -> 디렉셔널 라이트를 점점 밝게, 다른 라이트는 점점 어둡게
        foreach (var item in allLight)
        {
            item.Key.enabled = true; // 모든 라이트는 항샹 켜두고 점점 어둡게 할 것임
            if (item.Key.type == LightType.Directional)
                DOTween.To(() => 0f, (x) => item.Key.intensity = x, item.Value,changeDuration);
            else
                DOTween.To(() => item.Value, (x) => item.Key.intensity = x, 0, changeDuration);
        }
        // 백그라운드 색 ( 카메라에 보이는 ) 도 밤, 낮에 맞는 컬러로 바꿔주는 코드
        DOTween.To(() => Camera.main.backgroundColor, (x) => Camera.main.backgroundColor = x, dayColor, changeDuration);
    }
    
    
    private void ChangeNightLight()
    {
        if (allLight == null)
        {
            allLight = new Dictionary<Light, float>(); // 초기화 해주는 코드
            var _allLight = FindObjectsOfType<Light>();
            foreach (var item in _allLight)
            {
                allLight[item] = item.intensity;
            }
        }
        // 밤으로 변함 -> 디렉셔널 라이트를 점점 어둡게, 다른 라이트는 점점 밝게
        foreach (var item in allLight)
        {
            item.Key.enabled = true; // 모든 라이트는 항샹 켜두고 점점 어둡게 할 것임
            if (item.Key.type != LightType.Directional)
                DOTween.To(() => 0f, (x) => item.Key.intensity = x, item.Value, changeDuration);
            else
                DOTween.To(() => item.Value, (x) => item.Key.intensity = x, 0, changeDuration);
        }
        // 백그라운드 색 ( 카메라에 보이는 ) 도 밤, 낮에 맞는 컬러로 바꿔주는 코드
        DOTween.To(() => Camera.main.backgroundColor, (x) => Camera.main.backgroundColor = x, nightColor, changeDuration);

    }
}
