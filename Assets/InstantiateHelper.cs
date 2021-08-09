using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class InstantiateInfo
{
    public float ratio = 1f;
    public GameObject go;
}
public class InstantiateHelper : MonoBehaviour
{
    public static bool ApplicationQuit = false;
    private void OnApplicationQuit() => ApplicationQuit = true;
    // ApplicationQuit =어플리 케이션 나갈때
    public List<InstantiateInfo> dropItems;
    public enum EventType
    {
        OnDestroy,
        OnDisable,
        OnEnable,
    }
    public EventType eventType = EventType.OnDestroy;

    private void OnDestroy()
    {
        if (eventType != EventType.OnDestroy)
            return;

        if (ApplicationQuit)
            return; 

        InstantiateObjects();
    }

    private void OnEnable()
    {
        if (eventType != EventType.OnEnable)
            return;
        InstantiateObjects();
    }
    private void OnDisable()
    {
        if (eventType != EventType.OnDisable)
            return;
        InstantiateObjects();
    }

    void InstantiateObjects()
    {
        // 보상 아이템 생성
        foreach (var item in dropItems)
        {
            if (item.ratio < Random.Range(0, 1f))
                continue;
            Instantiate(item.go, transform.position, Quaternion.identity);
        }
    }
}
