using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEnemyShoot : MonoBehaviour
{    
    public Transform player;
    public GameObject bulletPrefab;
    public float shootForce;
    public Transform offset;
    //States
    public float sightRange;
    public bool playerInSightRange, canShoot;
    public float shootCooldown;
    void Start()
    {
        player = GameObject.Find("XR Origin").transform;
    }
    void Update()
    {
        if(playerInSightRange && canShoot)
        {
            ShootAtPlayer();
            Invoke(nameof(ResetCanShoot),shootCooldown);
        }
    }
    void ShootAtPlayer()
    {
        transform.LookAt(player);
        GameObject bullet = Instantiate(bulletPrefab,offset) as GameObject;
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
        bullet.transform.parent = null;
    }
    void ResetCanShoot()
    {
        canShoot = true;
    }
}
