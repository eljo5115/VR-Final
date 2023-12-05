using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.layer == 8)
        {
            GameManager.collectibleCount++;
        }
    }
}
