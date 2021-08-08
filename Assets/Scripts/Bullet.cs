using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20;
    public float destroyTime = 1f;
    public int power = 20;
    public int randomDamage = 3;
    public float pushBackDistance = 0.1f;
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        //transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.Self);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Zombie") // 비추천 -> GC발생.
        if(other.CompareTag("Zombie")) // 이 방식 추천 -> GC 발생 x
        {
            var zombie = other.GetComponent<Zombie>();
            //Vector3 toMoveDirection = transform.position - zombie.transform.position;
            //toMoveDirection.Normalize();
            zombie.TakeHit(power + Random.Range(-randomDamage, randomDamage)
                , transform.forward
                , pushBackDistance);
            Destroy(gameObject);
        }
    }
}
