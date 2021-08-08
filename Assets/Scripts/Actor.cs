using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int hp = 100;
    public float bloodEffectYPosition = 1.3f;

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
