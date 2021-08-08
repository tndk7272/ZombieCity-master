using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class MoveToPlayer : MonoBehaviour
{
    NavMeshAgent agent;
    public float maxSpeed = 20f;
    public float duration = 3f; // 3초동안 20으로 속도 증가  = 최대 speed 20 으로

    bool alreadyDone = false;
    TweenerCore<float, float, FloatOptions> tweenResult;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyDone) // alreadyDone이 실행됐따면
            return; // 코루틴 정지

        if (other.CompareTag("Player"))
        {
            alreadyDone = true;
            agent = GetComponent<NavMeshAgent>();
            

            // 스피드를 점점 높일것이다 // 스피드가 동일하면 동전이 플레이어를 못 따라가니까
           var tweenResult = DOTween.To(() => agent.speed, (x) => agent.speed = x, maxSpeed, duration);  // 3초간 1f 에서 20까지 올리며 그 값을 agent.speed에 할당하겠다
            tweenResult.SetLink(gameObject); // 게임오브젝트가 파괴될때 해당 트위닝도 같이 정지되게끔 하는 코드 // 두번째 파라미터에 여러가지 옵션이 있음 

            // DOTween.To(() => agent.speed, (x) => agent.speed = x, maxSpeed, duration).SetLink(gameObject);  
            // ㄴ 이거는 위에 두줄이랑 같은코드

            setDestinationCoHandle = StartCoroutine(SetDestinationCo(other.transform));

        }
       
    }

    public void StopCoroutine()
    {
        StopCoroutine(setDestinationCoHandle);
    }
    Coroutine setDestinationCoHandle;
    private IEnumerator SetDestinationCo(Transform tr)
    {
        while (true)
        {
            agent.destination = tr.position; // 타겟  지정해주는거
            yield return null; // 무한루프 쓸때는 이 코드 먼저 작성하고 코딩하자
        }
    }
}
