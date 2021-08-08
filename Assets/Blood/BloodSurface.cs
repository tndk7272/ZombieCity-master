using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSurface : MonoBehaviour
{
    List<ParticleCollisionEvent> collisionEvent = new List<ParticleCollisionEvent>();
    public GameObject groundBlood;
    private void OnParticleCollision(GameObject other)
    {
        var ps = other.GetComponent<ParticleSystem>();
        ParticlePhysicsExtensions.GetCollisionEvents(ps, gameObject, collisionEvent);
        //print(collisionEvent.Count);
        foreach (var item in collisionEvent)
        {
            //print(item.intersection);
            Quaternion rotate = Quaternion.Euler(0, item.velocity.VectorToDegree(), 0);
            Instantiate(groundBlood, item.intersection, rotate);
        }
    }
}
