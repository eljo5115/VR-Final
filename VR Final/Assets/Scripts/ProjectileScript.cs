using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public int objLifetime;
    void Start()
    {
        Destroy(gameObject,objLifetime);
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.collider.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
