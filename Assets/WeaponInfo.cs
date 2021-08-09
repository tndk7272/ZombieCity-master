using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public enum WeaponType
    {      
        Gun,
        Melee, // 근접공격, 총알 없음 다른것도 다 없음
    }
    public WeaponType type;


    // 공통으로 있는 것들
    public int damage = 20;
    public AnimatorOverrideController overrideAnimator;
    public float delay = 0.2f;
    public float pushBackDistance = 0.1f;


    public int bulletCountInClip = 2;       // 탄창에 총알수
    public int maxBulletCountInClip = 6;    // 탄창에 들어가는 최대수
    public int allBulletCount = 500;       // 가진 전체 총알수
    public int maxBulletCount = 500;
    public float reloadTime = 1f;

    // 총
    [Header("총")]
    public GameObject bullet;
    public Transform bulletPosition;
    public Light bulletLight;


    // 밀리어택 
    [Header("빠따")]
    public float attackStartTime = 0.1f; // 휘둘러서 어택 컬라이더가 활성화 되는 시간
    public float attackTime = 0.4f;      // 공격이 진행되고 있는 시간
    public Collider attackCollider;

}
