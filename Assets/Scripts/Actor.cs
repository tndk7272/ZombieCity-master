using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int hp = 100;
    [HideInInspector] public int maxHp;// 개발자한테 안보여주게 할 것임
    public float bloodEffectYPosition = 1.3f;

    protected void Awake()
    {// player(자식) 에 엑터가 있따면 엑터에 있는 어웨이크가 실행이 안된다.
        // 그렇기 때문에 항상 부모에 있는 애들은 protexted로ㅓ 바꿔줘야함
        maxHp = hp;
    }

    public GameObject bloodParticle;
    protected Animator animator;
    protected void CreateBloodEffect() // 자식들도 쓰기 위해서 protected 로 바꿔줌
    {
        // 좀비한테 피가 나오게하는거
        var pos = transform.position;
        pos.y = bloodEffectYPosition;
        Instantiate(bloodParticle, pos, Quaternion.identity);
    }

    public static void CreateTextEffect(int number, Vector3 position, Color color)
    {        
        GameObject memoryGo = (GameObject)Resources.Load("TextEffect");
        GameObject go = Instantiate(memoryGo, position, Camera.main.transform.rotation); 
        TextMeshPro textMeshPro = go.GetComponent<TextMeshPro>();
        textMeshPro.text = number.ToNumber();
        textMeshPro.color = color;
    }

    public Color damageColor = Color.white; // 기본 색
    protected void TakeHit(int damage)
    {
        hp -= damage;
        CreateBloodEffect();// 피 생성
        CreateTextEffect(damage,transform.position, damageColor);
    }


}
