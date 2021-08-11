using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int currentWaveIndex; // 웨이브 할것임 첫번째 웨이브 두번째 웨이브 - - -

    [System.Serializable]
    public class RegenInfo   // 몬스터리젠 정보
    {
        public GameObject monster;
        public float ratio; // 확률
    }

    [System.Serializable]

    public class WaveInfo
    {
        public int spawnCount = 10;
        // 웨이브당 몇마리의 , 어떤 몬스터가 생성될지
        public GameObject monster;
        public float time;

    }

    public List<WaveInfo> waves;


    IEnumerator Start()
    {
        var spawnPoints = GetComponentsInChildren<SpawnPoint>(true);
        foreach (var item in waves) // 특정시간이 끝난 다음에 또는 모든 몬스터가 다 죽었다면 다음 웨이브
        {
            Debug.LogWarning($"{currentWaveIndex} 시작됨");
            int spawnCount = item.spawnCount;
            for (int i = 0; i < spawnCount; i++)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Length);
                Vector3 spawnPoint = spawnPoints[spawnIndex].transform.position;
                Instantiate(item.monster, spawnPoint, Quaternion.identity);
            }

            float newxtWaveStartTime = Time.time + item.time;   // 현재 시간 + 지금 웨이브의 시간  
            while (Time.time < newxtWaveStartTime)   // 현재 시간이 넥스트 타임보다 작다면 기다리자
                yield return null;

            


            // 웨이브 바뀔때마다 밤 낮 바뀌게 하자 1라운드 = 아침 , 2라운드 = 밤
            LightManager.Instance.ToggleLight();
        }

    }
}