using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 키보드 누르면 씬 넘어가게
public class TitleScene : MonoBehaviour
{
    public LoadingUI loadingUI;
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            var progress = SceneManager.LoadSceneAsync("Stage1"); // 비동기로 씬을 로드 하는 코드
            loadingUI.ShowProgress(progress);
            //progress.progress // 0이면 작업 중 1이면 작업 끝
            //progress.isDone // 트루면 비동기 로직이 끝났다는 뜻
        }
    }
}
