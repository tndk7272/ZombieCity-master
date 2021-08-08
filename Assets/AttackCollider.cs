using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie") == false)
            return;


        GetComponentInParent<Player>().OnZombieEnter(other);
    }
}
