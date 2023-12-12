using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private GameObject item;
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.CompareTag("Player"))
        {
            GameManager.collectibleCount++;
            Destroy(gameObject);
        }
    }
    public void PickUpItem()
    {
        Debug.Log("item pickup");
        GameManager.collectibleCount++;
        Destroy(item);
    }
}
