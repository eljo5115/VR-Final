using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBomb : MonoBehaviour
{
    [SerializeField] private float objLifetime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, objLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision other)
    {
        //sphere cast for damage 
        // particle effect?
        Debug.Log("Bomb blow up");
        Destroy(gameObject);
    }
}
