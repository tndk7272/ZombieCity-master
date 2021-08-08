using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
public partial class Player : Actor
{
    public enum StateType
    {
        Idle,
        Move,
        TakeHit,
        Roll,
        Die,
        Reload,
    }



    public bool isFiring = false;
    public StateType stateType = StateType.Idle;

    public WeaponInfo mainWeapon;
    public WeaponInfo subWeapon;

    public WeaponInfo currentWeapon;
    public Transform rightWeaponPosition;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        ChangeWeapon(mainWeapon);

        SetCinemachinCamera();
    }

    GameObject currentWeaponGo;

    private void ChangeWeapon(WeaponInfo _weaponInfo)
    {
        Destroy(currentWeaponGo);
        currentWeapon = _weaponInfo;

        animator.runtimeAnimatorController = currentWeapon.overrideAnimator;

        var weaponInfo = Instantiate(currentWeapon, rightWeaponPosition);
        currentWeaponGo = weaponInfo.gameObject;

        weaponInfo.transform.localScale = currentWeapon.gameObject.transform.localScale;
        weaponInfo.transform.localPosition = currentWeapon.gameObject.transform.localPosition;
        weaponInfo.transform.localRotation = currentWeapon.gameObject.transform.localRotation;
        currentWeapon = weaponInfo;

        if (currentWeapon.attackCollider)
            currentWeapon.attackCollider.enabled = false;

        bullet = weaponInfo.bullet;
        bulletPosition = weaponInfo.bulletPosition;

        if (weaponInfo.bulletLight != null)   // bulletLight 는 유티니 오브젝트이기 때문에.... ?     물음표문법을 쓰면 안댄다ㅏ
            bulletLight = weaponInfo.bulletLight.gameObject;
        shootDelay = currentWeapon.delay; // 슛 딜레이 만큼 다시 클릭하면 공격 안되도록

    }

    [ContextMenu("SetCinemachinCamera")]
    private void SetCinemachinCamera()
    {
        // 카메라가 항상 플레이어를 찾도록
        var vcs = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var item in vcs)
        {
            item.Follow = transform;
            item.LookAt = transform;
        }
    }

    void Update()
    {

        if (Time.deltaTime == 0)
            return;

        if (stateType == StateType.Die)
            return;

        if (stateType != StateType.Roll)
        {
            LookAtMouse();
            Move();
            Fire();
            Roll();  // Roll중에는 Roll을 못하도록
            ReloadBullet();
            if (Input.GetKeyDown(KeyCode.Tab))
                ToggleChangeWeapon();
        }
    }

    private void ReloadBullet()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ReloadBulletCo());
        }
    }

    private IEnumerator ReloadBulletCo()
    {
        stateType = StateType.Reload;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime); 
        stateType = StateType.Idle;
        int reloadcount = Math.Min(allBulletCount, MaxBulletCountInClip); // 더 작은 숫자를 리턴한다
        bulletCountInClip = reloadcount;
        allBulletCount -= reloadcount;
        

    }

    bool toggleWeapon = false;
    void ToggleChangeWeapon()
    {
        ChangeWeapon(toggleWeapon == true ? mainWeapon : subWeapon);
        toggleWeapon = !toggleWeapon;
    }


    private void Roll()  // 구르는 거
    {

        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(RollCo());


    }

    // 애니메이션 커브로 rollingSpeed속도를 바꾸자
    public AnimationCurve rollingSpeedAc;
    public float rollingSpeedUserMultipy = 1;


    private IEnumerator RollCo()
    {
        EndFiring();
        // 회전 방향은 처음 바라보던 방향으로 고정
        // 구르는 동안 총알 금지, 움직이는거 금지, 바라보는거 금지
        stateType = StateType.Roll;
        animator.SetTrigger("Roll");
        // 구르는 동안 이동 스피드를 빠르게
        float starTime = Time.time;
        float endTime = starTime + rollingSpeedAc[rollingSpeedAc.length - 1].time;
        while (endTime > Time.time)
        {
            float time = Time.time - starTime;
            float rollingSpeedMultipy = rollingSpeedAc.Evaluate(time) * rollingSpeedUserMultipy;
            // print($"{time}:{rollingSpeedMultipy} : {rollingSpeedAc[rollingSpeedAc.length - 1].time}");

            transform.Translate(transform.forward * speed * rollingSpeedMultipy * Time.deltaTime, Space.World);
            yield return null;
        }
        stateType = StateType.Idle;


    }



    Plane plane = new Plane(new Vector3(0, 1, 0), 0);

    private void LookAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dir = hitPoint - transform.position;
            dir.y = transform.position.y;
            dir.Normalize();
            transform.forward = dir;
        }
    }


    private void Move()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move.z = 1;
        if (Input.GetKey(KeyCode.S)) move.z = -1;
        if (Input.GetKey(KeyCode.A)) move.x = -1;
        if (Input.GetKey(KeyCode.D)) move.x = 1;
        if (move != Vector3.zero)
        {
            // 카메라를 기준으로 움직이게 하기
            Vector3 relateMove;
            relateMove = Camera.main.transform.forward * move.z;  // 카메라의 forward 는 z 방향이다
            relateMove += Camera.main.transform.right * move.x; // 기존에 있떤 값을 더할것이다
            relateMove.y = 0; // y는 안쓸거니까
            move = relateMove;

            move.Normalize();

            float _speed = isFiring ? speedWhileShooting : speed;

            transform.Translate(move * _speed * Time.deltaTime, Space.World);


            if (Mathf.RoundToInt(transform.forward.x) == 1 || Mathf.RoundToInt(transform.forward.x) == -1)
            {
                animator.SetFloat("DirX", transform.forward.z * move.z);
                animator.SetFloat("DirY", transform.forward.x * move.x);
            }
            else
            {
                animator.SetFloat("DirX", transform.forward.x * move.x);
                animator.SetFloat("DirY", transform.forward.z * move.z);
            }

        }
        animator.SetFloat("Speed", move.sqrMagnitude);
    }



    new internal void TakeHit(int damage)
    {
        base.TakeHit(damage);

        animator.SetTrigger("TakeHit");

        if (hp <= 0)
        {
            StartCoroutine(DieCo());

        }
    }

    public float diePreDelayTime = 0.3f;
    private IEnumerator DieCo()
    {
        stateType = StateType.Die;
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(diePreDelayTime);
        animator.SetTrigger("Die");
    }

    public float speed = 5;
    public float speedWhileShooting = 3;
    public void OnZombieEnter(Collider other)  // 좀비가 들어와따
    {
        var zombie = other.GetComponent<Zombie>();
        zombie.TakeHit(currentWeapon.damage
            , currentWeapon.gameObject.transform.forward
            , currentWeapon.pushBackDistance);

    }

}
