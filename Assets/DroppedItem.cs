using UnityEngine;


public enum DropItemType
{
    Gold,
    Point,
    Item,
}
public class DroppedItem : MonoBehaviour
{
    public enum GetMethodType  
    {
        TriggerEnter,   // 아이템 자석효과
        KeyDown,        // 수동으로 먹기
    }
    public GetMethodType getMethod;
    public KeyCode keyCode = KeyCode.E;  // 가까이 가서 E 키 누르며 ㄴ획득하도록 

    public DropItemType type;
    public int amount; // 수량이 얼마나 떨어졌는지
    public int itemId;

    bool alreadyDone = false;
    private void Awake()
    {
        enabled = false;  // 처음시작하면 enabled 끄기

    }
    private void OnTriggerEnter(Collider other)
    {
        if (alreadyDone)
            return;

        if (other.CompareTag("Player"))
        {
            switch (getMethod)
            {
                case GetMethodType.TriggerEnter:
                    GetItem();
                    break;
                case GetMethodType.KeyDown:
                    enabled = true;
                    break;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enabled = false;  // 이 지역을 나가면 트리거를 끄는거 ?..
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            enabled = false;
            GetItem();
        }
    }
    public Color color = Color.white;
    private void GetItem()   // 획득 완료
    {
        alreadyDone = true; // 아이템 획득했을때만 실행
        switch (type)
        {
            case DropItemType.Gold:  // 타입이 골드일때 플레이어에게 골드 증가, UI에 반영
                Actor.CreateTextEffect(amount, transform.position, color);
                StageManager.Instance.AddGold(amount);
                break;

        }
        transform.GetComponentInParent<MoveToPlayer>()?.StopCoroutine();
        Destroy(transform.root.gameObject);
    }
}

