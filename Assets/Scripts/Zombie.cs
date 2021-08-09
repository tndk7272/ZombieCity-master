using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
public class Zombie : Actor
{

    public Transform target;
    NavMeshAgent agent;
    float originalSpeed;
    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        target = FindObjectOfType<Player>().transform;  // 
        originalSpeed = agent.speed;
        attackCollider = transform.Find("AttackRange").GetComponent<SphereCollider>();


        CurrentFsm = ChaseFSM;
        while (true)  // 상태를 무한히 반복해서 실행하는 부분
        {
            var previousFSM = CurrentFsm;

            fsmHandle = StartCoroutine(CurrentFsm());

            // FSM 안에서 에러 발생시 무한 루프 도는 것을 방지 하기 위해서 추가함.
            if (fsmHandle == null && previousFSM == CurrentFsm)
                yield return null;

            while (fsmHandle != null)
                yield return null;

        }
    }
    Coroutine fsmHandle;
    protected Func<IEnumerator> CurrentFsm
    {
        get { return m_currentFsm; }
        set
        {
            if (fsmHandle != null)
                StopCoroutine(fsmHandle);

            m_currentFsm = value;
            fsmHandle = null;
        }
    }
    Func<IEnumerator> m_currentFsm;

    IEnumerator ChaseFSM()
    {
        if (target)
            agent.destination = target.position;
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));

        SetFsm_SelectAttackTargetOrAttackOrChase();

    }

    private void SetFsm_SelectAttackTargetOrAttackOrChase()
    {
        if (IsAttackableTarget())     // 만약 타겟이 공격 할 수 없는 상태라면
        {
            // 타겟이 공격 범위 안에 들어왔는가 ??
            if (TargetIsInAttackArea()) // 들어왔다면 
                CurrentFsm = AttackFsm;    // CurrentFsm 을 AttackFsm 으로
            else
                CurrentFsm = ChaseFSM;
        }
        else
        {
            print("배회하기 구현 해야ㅏ함");
            // 다른 타겟을 찾거나

            // 공격 가능한 타겟이 없다면
            // -> 배회하기 혹은 제자리 가만히 있기.
        }
    }



    private bool IsAttackableTarget()
    {
        if (target.GetComponent<Player>().stateType == Player.StateType.Die)
            return false;

        return true;
    }

    float attackDistance = 3f;
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    private bool TargetIsInAttackArea()
    {

        // 일단 거리를 잴 것임
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < attackDistance;  // 타겟이 어택 범위에 들어왔다면 트루
    }

    public float attackTime = 0.4f;   // 0.4초때 공격 애니메이션 ( 공격 타이밍 )
    public float attackAnimationTime = 0.8f;  // 전체 공격 애니메이션 시간
    public SphereCollider attackCollider;
    public LayerMask enemyLayer;
    public int power = 20;
    private IEnumerator AttackFsm()
    {

        // 범위 안에 들어왔다면 타겟을 바라보자
        var looAtPosition = target.position;
        looAtPosition.y = transform.position.y; // y는 그대로  
        transform.LookAt(looAtPosition);

        // 공격 애니메이션 하기
        animator.SetTrigger("Attack");

        // 공격 중에는 이동 스피드 0
        agent.speed = 0;

        // 공격타이밍까지 대기 
        yield return new WaitForSeconds(attackTime);
        // 특정 시간(공격타이밍) 지나면 충돌 메시 사용하여 충돌 감지하기

        // 충돌 메시 사용해서 충돌 감지
        Collider[] enemyColliders = Physics.OverlapSphere(
            attackCollider.transform.position, attackCollider.radius, enemyLayer);
        foreach (var item in enemyColliders)
        {
            item.GetComponent<Player>().TakeHit(power);
        }

        // 애니메이션 끝날때까지 대기
        yield return new WaitForSeconds(attackAnimationTime - attackTime);

        // 이동 스피드 복구
        SetOriginalSpeed();


        // Fsm 지정해주기
        SetFsm_SelectAttackTargetOrAttackOrChase();

    }

    internal void TakeHit(int damage, Vector3 toMoveDirection
        , float pushBackDistance = 0.1f)
    {
        base.TakeHit(damage);
        if (hp <= 0)
        {
            GetComponent<Collider>().enabled = false;
            animator.SetBool("Die", true);
        }

        // 뒤로 밀려나게하자.
        PushBackMove(toMoveDirection, pushBackDistance);

        CurrentFsm = TakeHitFSM;
    }

    IEnumerator TakeHitFSM()
    {
        animator.Play(Random.Range(0, 2) == 0 ? "TakeHit1" : "TakeHit2", 0, 0);

        // 이동 스피드를 잠시 0으로 만들자.
        agent.speed = 0;

        yield return new WaitForSeconds(TakeHitStopSpeedTime); // 피격 모션 끝나기까지 기다림.

        if (hp <= 0)
        {
            Die();
            yield break;
        }
        else
        {
            SetOriginalSpeed();
        }

        // 이 Fsm 이 끝났으니 다른 Fsm 으로 돌리자 
        // 추격하는 Fsm 으로 돌릴 것 임
        CurrentFsm = ChaseFSM;
    }
    public float moveBackDistance = 1f;
    public float moveBackNoise = 0.1f;
    public float moveBackDuration = 0.5f;
    public Ease moveBackEase = Ease.OutQuart;  // 닷트윈 그래프
    private void PushBackMove(Vector3 toMoveDirection, float _moveBackDistance)
    {
        toMoveDirection.x += Random.Range(-moveBackNoise, moveBackNoise);
        toMoveDirection.z += Random.Range(-moveBackNoise, moveBackNoise);
        toMoveDirection.y = 0;
        toMoveDirection.Normalize();

        transform.DOMove(transform.position +
            toMoveDirection * _moveBackDistance * moveBackDistance, moveBackDuration)
            .SetEase(moveBackEase);
    }


    public float TakeHitStopSpeedTime = 0.1f;
    public float onDieDelayDestroy = 2f;
    private void SetOriginalSpeed()
    {
        agent.speed = originalSpeed;
    }
    public int rewardScore = 100; // 몬스터마다 다른 점수를 주기 위한 변수
    public Material dieMaterial; // 죽을때 메테리얼 
    public float dieMaterialDuration = 2; // 2초 동안 바뀌게 할것이다

    void Die()
    {
        StageManager.Instance.AddScore(rewardScore); // 좀비 죽일때마다 점수 100점

        // 메테리얼 교체
        // 메테리얼 가져오쟝
        var renderers = GetComponentsInChildren<Renderer>(true);
        foreach (var item in renderers)
        {
            item.sharedMaterial = dieMaterial;
        }

        dieMaterial.SetFloat("_Progress", 1);
        // 좀비가 완전히 사라졌는데 코인이 좀 늦게 떨어지기 때문에 0.14를 endvalue로 넣어쥼
        DOTween.To(() => 1f, (x) => dieMaterial.SetFloat("_Progress", x), 0.14f, dieMaterialDuration)
            .SetDelay(onDieDelayDestroy)
            .OnComplete(() => Destroy(gameObject));


        // 교체되는 동안 보여주ㅗ고 파괴


    }
}
