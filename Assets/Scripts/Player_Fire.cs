using System.Collections;
using UnityEngine;

public partial class Player : Actor
{
    // 람다식으로 속성으로 만들어줌 ( 읽기 전용 )
    public int BulletCountInClip
    {  // 탄창에 총알수
        get { return currentWeapon.bulletCountInClip; }
        set { currentWeapon.bulletCountInClip = value; }
    }
    public int MaxBulletCountInClip => currentWeapon.maxBulletCountInClip;    // 탄창에 들어가는 최대수
    public int AllBulletCount
    {
        get => currentWeapon.allBulletCount;      // 가진 전체 총알수.
        set => currentWeapon.allBulletCount = value;
        // 쓰기도 가능한 속성
    }

    public int MaxBulletCount => currentWeapon.maxBulletCount;      // 최대로 가질 수 있는 총알수.
    public float ReloadTime => currentWeapon.reloadTime;

    public GameObject Bullet => currentWeapon.bullet;
    public Transform BulletPosition => currentWeapon.bulletPosition;


    float shootDelayEndTime;
    void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            // 만약 사용할 수 있는 총알이 있다면 ?
            if (BulletCountInClip > 0)
            {
                isFiring = true;
                if (shootDelayEndTime < Time.time && BulletCountInClip > 0)
                {
                    BulletCountInClip--;

                    animator.SetTrigger("StartFire");

                    AmmoUI.Instance.SetBulletCount(BulletCountInClip
                        , MaxBulletCountInClip
                        , AllBulletCount + BulletCountInClip
                        , MaxBulletCount);

                    shootDelayEndTime = Time.time + shootDelay;
                    switch (currentWeapon.type)
                    {
                        case WeaponInfo.WeaponType.Gun:// 만약에 현재 웨폰에 웨폰 타입이 총일때 
                            IncreaseRecoil();
                            currentWeapon.StartCoroutine(InstantiateBulletAndFlashBulletCo());
                            break;

                        case WeaponInfo.WeaponType.Melee:
                            // 무기의 컬리아더를 활성화하자
                            currentWeapon.StartCoroutine(MeleeAttackCo());
                            break;
                    }

                }
            }
            else
            {
                if( reloadAlertDelayEndTime < Time.time)
                {
                    reloadAlertDelayEndTime = Time.time + reloadAlertDelay;
                    // 리로드 글자를 표시하자
                    CreateTextEffect("Reload!", "TalkEffect", transform.position, Color.white); 
                }
                
            }

        }
        else
        {
            EndFiring();
        }
    }

    [SerializeField] float reloadAlertDelay = 0.5f;
    float reloadAlertDelayEndTime;

    private void CreateReloadText()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator MeleeAttackCo()
    {
        // 무기의 컬리아더를 활성화하자
        yield return new WaitForSeconds(currentWeapon.attackStartTime);
        currentWeapon.attackCollider.enabled = true;
        yield return new WaitForSeconds(currentWeapon.attackTime);
        currentWeapon.attackCollider.enabled = false;

    }
    private void EndFiring()
    {
        // 구르는 애니메이션 재생. // 총쏘는ㄱ[ 끝났을때 시작되는 코드
        DecreaseRecoil();
        isFiring = false;
    }

    GameObject bulletLight;
    public float bulletFlashTime = 0.001f;
    private IEnumerator InstantiateBulletAndFlashBulletCo()
    {
        yield return null; // 총쏘는 애니메이션 시작후에 총알 발사하기 위해서 1Frame쉼
        GameObject bulletGo = Instantiate(Bullet, BulletPosition.position, CalculateRecoil(transform.rotation));
        bulletGo.GetComponent<Bullet>().pushBackDistance = currentWeapon.pushBackDistance;

        bulletLight.SetActive(true);
        yield return new WaitForSeconds(bulletFlashTime);
        bulletLight.SetActive(false);


    }

    float recoilValue = 0f;
    float recoilMaxValue = 1.5f;
    float recoilLerpValue = 0.1f;
    void IncreaseRecoil()
    {
        recoilValue = Mathf.Lerp(recoilValue, recoilMaxValue, recoilLerpValue);
    }
    void DecreaseRecoil()
    {
        recoilValue = Mathf.Lerp(recoilValue, 0, recoilLerpValue);

    }

    Vector3 recoil;
    Quaternion CalculateRecoil(Quaternion rotation)
    {
        recoil = new Vector3(Random.Range(-recoilValue, recoilValue), Random.Range(-recoilValue, recoilValue), 0);
        return Quaternion.Euler(rotation.eulerAngles + recoil);
    }

    [SerializeField] float shootDelay = 0.05f;
}
